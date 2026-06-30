using Order.Domain.Exceptions;

namespace Order.Domain.ValueObjects;

public record ProductId
{
    public Guid Value { get; init; }
    public static ProductId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("ProductId cannot be empty.");
        }

        return new ProductId { Value = value };
    }
}