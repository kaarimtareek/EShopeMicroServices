namespace Order.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string message) : base($"DomainException: {message}")
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}