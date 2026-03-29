using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary contains the expected value.
/// </summary>
internal class DictionaryContainValueValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain,
    TValue value
) : IValidator, IRuleDescriptor
    where TKey : notnull
{
    public static DictionaryContainValueValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain,
        TValue value
    ) => new(chain, value);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Dictionary.ContainValue";
    string IRuleDescriptor.OperationName => "ContainValue";
    Type IRuleDescriptor.SubjectType => typeof(IDictionary<,>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = value! };

    public bool Validate()
    {
        if (chain.GetValue().Values.Contains(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting dictionary was expected to contain value {0}, but it did not.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
