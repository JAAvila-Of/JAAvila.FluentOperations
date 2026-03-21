using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary reference does not equal the expected reference (reference equality).
/// </summary>
internal class DictionaryNotBeValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain,
    IDictionary<TKey, TValue> expected
) : IValidator, IRuleDescriptor
    where TKey : notnull
{
    public static DictionaryNotBeValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain,
        IDictionary<TKey, TValue> expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Dictionary.NotBe";
    string IRuleDescriptor.OperationName => "NotBe";
    Type IRuleDescriptor.SubjectType => typeof(IDictionary<,>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!ReferenceEquals(value, expected))
        {
            return true;
        }

        ResultValidation =
            "The resulting dictionary was expected to not be the same reference as {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
