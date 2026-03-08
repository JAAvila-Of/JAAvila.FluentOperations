namespace JAAvila.FluentOperations.Model;

internal class BaseValue<T>(T value)
{
    public T Value { get; set; } = value;

    public static BaseValue<T> Create(T value) => new(value);
}
