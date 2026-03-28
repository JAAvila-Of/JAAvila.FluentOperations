using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary contains the specified key-value pair.
/// </summary>
internal class DictionaryContainPairValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain,
    TKey key,
    TValue value
) : IValidator, IRuleDescriptor
    where TKey : notnull
{
    public static DictionaryContainPairValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain,
        TKey key,
        TValue value
    ) => new(chain, key, value);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Dictionary.ContainPair";
    string IRuleDescriptor.OperationName => "ContainPair";
    Type IRuleDescriptor.SubjectType => typeof(IDictionary<,>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["key"] = key, ["value"] = value! };

    public bool Validate()
    {
        var dict = chain.GetValue();

        if (dict.TryGetValue(key, out var v) && EqualityComparer<TValue>.Default.Equals(v, value))
        {
            return true;
        }

        ResultValidation =
            "The resulting dictionary was expected to contain pair [{0} => {1}], but it did not.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
