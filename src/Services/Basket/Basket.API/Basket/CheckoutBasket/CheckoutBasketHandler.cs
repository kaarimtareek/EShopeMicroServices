using Basket.API.Data;
using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public sealed class CheckoutBasketCommand : ICommand<CheckoutBasketResult>
{
    public CheckoutBasketCommand()
    {
    }

    public CheckoutBasketCommand(BasketCheckoutDto basketCheckoutDto)
    {
        BasketCheckoutDto = basketCheckoutDto;
    }

    public BasketCheckoutDto BasketCheckoutDto { get; set; }
}

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("The basket must not be null.");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotNull().NotEmpty().WithMessage("The user name must not be empty.");
    }
}

public sealed record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(request.BasketCheckoutDto.UserName, cancellationToken);
        if (basket == null)
            return new CheckoutBasketResult(false);
        var eventMessage = request.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;
        await publishEndpoint.Publish(eventMessage, cancellationToken);
        return new CheckoutBasketResult(true);
    }
}