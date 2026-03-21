using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary has the expected number of elements.
/// </summary>
internal class DictionaryHaveCountValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain,
    int expected
) : IValidator, IRuleDescriptor where TKey : notnull
{
    public static DictionaryHaveCountValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain,
        int expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Dictionary.HaveCount";
    string IRuleDescriptor.OperationName => "HaveCount";
    Type IRuleDescriptor.SubjectType => typeof(IDictionary<,>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue().Count == expected)
        {
            return true;
        }

        ResultValidation = "The resulting dictionary was expected to have {0} entry(ies), but it had {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
