using Basket.API.Data;

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

public class StoreBasketCommandHandler(IBasketRepository repository)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        //repository
        var shoppingCart = await repository.StoreBasketAsync(command.Cart, cancellationToken);
        if (shoppingCart == null)
            return new StoreBasketResult(null);
        return new StoreBasketResult(shoppingCart.UserName);
    }
}