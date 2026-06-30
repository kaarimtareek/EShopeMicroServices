using Order.Domain.Exceptions;

namespace Order.Domain.ValueObjects;

public record OrderId
{
    public Guid Value { get; init; }

    public static OrderId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("OrderId cannot be empty.");
        }

        return new OrderId { Value = value };
    }
}