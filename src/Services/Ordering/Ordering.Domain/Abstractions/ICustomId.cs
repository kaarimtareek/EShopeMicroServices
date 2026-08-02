namespace Order.Domain.Abstractions;

public interface ICustomId<T>
{
    T Value { get; }
}