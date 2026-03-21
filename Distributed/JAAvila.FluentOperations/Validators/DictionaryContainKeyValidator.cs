using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary contains the expected key.
/// </summary>
internal class DictionaryContainKeyValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain,
    TKey key
) : IValidator, IRuleDescriptor where TKey : notnull
{
    public static DictionaryContainKeyValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain,
        TKey key
    ) => new(chain, key);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Dictionary.ContainKey";
    string IRuleDescriptor.OperationName => "ContainKey";
    Type IRuleDescriptor.SubjectType => typeof(IDictionary<,>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = key };

    public bool Validate()
    {
        if (chain.GetValue().ContainsKey(key))
        {
            return true;
        }

        ResultValidation = "The resulting dictionary was expected to contain key {0}, but it did not.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
