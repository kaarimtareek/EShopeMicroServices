namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(o => o.Order.Id).NotEmpty().WithMessage("Order Id is required");
        RuleFor(o => o.Order.CustomerId).NotEmpty().WithMessage("CustomerId is required");
        RuleFor(o => o.Order.OrderName).NotEmpty().WithMessage("Order name cannot be empty");
        RuleFor(o => o.Order.OrderItems).NotEmpty().WithMessage("OrderItems cannot be empty");
    }
}