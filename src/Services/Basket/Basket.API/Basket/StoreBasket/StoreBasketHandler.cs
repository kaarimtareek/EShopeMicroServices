using Basket.API.Data;
using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null.");
        RuleFor(x => x.Cart.UserName).NotNull().WithMessage("Username cannot be null.");
        RuleFor(x => x.Cart.Items).NotEmpty().WithMessage("Cart must contain at least one item.");
    }
}

public class StoreBasketCommandHandler(
    IBasketRepository repository,
    DiscountService.DiscountServiceClient discountService)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        //repository
        await DeductDiscount(command.Cart, cancellationToken);

        var shoppingCart = await repository.StoreBasketAsync(command.Cart, cancellationToken);
        if (shoppingCart == null)
            return new StoreBasketResult(null);
        return new StoreBasketResult(shoppingCart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        if (cart is null or { Items: null or { Count: 0 } })
            return;
        foreach (var item in cart.Items)
        {
            var discount =
                await discountService.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName },
                    cancellationToken: cancellationToken);
            if (discount is null or { Amount: <= 0 })
                continue;
            item.Price -= (decimal)discount.Amount;
        }
    }
}