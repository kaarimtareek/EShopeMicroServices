using Order.Domain.Exceptions;

namespace Order.Domain.ValueObjects;

public record OrderName
{
    private const int DefaultMaxLength = 100;
    public string Value { get; init; }

    private OrderName(string value)
    {
        Value = value;
    }

    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, DefaultMaxLength, nameof(value));


        return new OrderName(value);
    }
}