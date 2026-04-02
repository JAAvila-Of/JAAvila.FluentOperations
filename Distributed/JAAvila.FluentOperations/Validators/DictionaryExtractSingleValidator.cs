using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary contains exactly one entry and captures it for extraction.
/// </summary>
internal class DictionaryExtractSingleValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain
) : IValidator, IRuleDescriptor
    where TKey : notnull
{
    public static DictionaryExtractSingleValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain
    ) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Dictionary.ExtractSingle";
    string IRuleDescriptor.OperationName => "ExtractSingle";
    Type IRuleDescriptor.SubjectType => typeof(IDictionary<,>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    /// <summary>
    /// The extracted key-value pair after successful validation.
    /// </summary>
    public KeyValuePair<TKey, TValue> ExtractedValue { get; private set; }

    public bool Validate()
    {
        var dict = chain.GetValue();

        if (dict.Count == 1)
        {
            ExtractedValue = dict.First();
            return true;
        }

        ResultValidation =
            "Expected the dictionary to contain exactly one entry, but found {0} entries.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
