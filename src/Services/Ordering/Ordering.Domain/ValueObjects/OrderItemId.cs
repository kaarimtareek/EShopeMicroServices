using Order.Domain.Exceptions;

namespace Order.Domain.ValueObjects;

public record OrderItemId
{
    public Guid Value { get; init; }
    public static OrderItemId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("OrderItemId cannot be empty.");
        }

        return new OrderItemId { Value = value };
    }
}