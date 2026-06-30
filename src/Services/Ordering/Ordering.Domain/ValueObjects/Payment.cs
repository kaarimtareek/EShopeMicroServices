namespace Order.Domain.ValueObjects;

public record Payment
{
    public string CardName { get; } = null!;
    public string CardNumber { get; } = null!;
    public string CardExpirationDate { get; } = null!;
    public string CVV { get; } = null!;
    public int PaymentMethod { get; } = 0!;

    private Payment(string cardName, string cardNumber, string cardExpirationDate, string cVV, int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        CardExpirationDate = cardExpirationDate;
        CVV = cVV;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(string cardName, string cardNumber, string cardExpirationDate, string cVV,
        int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardExpirationDate);
        ArgumentException.ThrowIfNullOrWhiteSpace(cVV);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cVV.Length, 3);
        return new Payment(cardName, cardNumber, cardExpirationDate, cVV, paymentMethod);
    }
}