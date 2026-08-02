using Order.Domain.Exceptions;

namespace Order.Domain.ValueObjects;

public record CustomerId
{
    public Guid Value { get; init; }

    public static CustomerId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("CustomerId cannot be empty.");
        }

        return new CustomerId { Value = value };
    }
}