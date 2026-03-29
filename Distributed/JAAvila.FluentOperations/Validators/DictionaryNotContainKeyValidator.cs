using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary does not contain the expected key.
/// </summary>
internal class DictionaryNotContainKeyValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain,
    TKey key
) : IValidator, IRuleDescriptor
    where TKey : notnull
{
    public static DictionaryNotContainKeyValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain,
        TKey key
    ) => new(chain, key);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Dictionary.NotContainKey";
    string IRuleDescriptor.OperationName => "NotContainKey";
    Type IRuleDescriptor.SubjectType => typeof(IDictionary<,>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = key };

    public bool Validate()
    {
        if (!chain.GetValue().ContainsKey(key))
        {
            return true;
        }

        ResultValidation =
            "The resulting dictionary was expected to not contain key {0}, but it did.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
