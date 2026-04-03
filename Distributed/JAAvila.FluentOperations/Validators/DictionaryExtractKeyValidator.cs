using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary contains the specified key and captures the associated value for extraction.
/// </summary>
internal class DictionaryExtractKeyValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain,
    TKey key
) : IValidator, IRuleDescriptor
    where TKey : notnull
{
    public static DictionaryExtractKeyValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain,
        TKey key
    ) => new(chain, key);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Dictionary.ExtractKey";
    string IRuleDescriptor.OperationName => "ExtractKey";
    Type IRuleDescriptor.SubjectType => typeof(IDictionary<,>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["key"] = key };

    /// <summary>
    /// The extracted value after successful validation.
    /// </summary>
    public TValue? ExtractedValue { get; private set; }

    public bool Validate()
    {
        var dict = chain.GetValue();

        if (dict.ContainsKey(key))
        {
            ExtractedValue = dict[key];
            return true;
        }

        ResultValidation =
            "Expected the dictionary to contain key {0}, but it was not found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
