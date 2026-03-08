namespace JAAvila.FluentOperations.Model;

internal record BaseChain<T>(BaseValue<T> BaseValue, BaseSubject BaseSubject)
{
    public static BaseChain<T> Create(BaseValue<T> value, BaseSubject subject) =>
        new(value, subject);
}
