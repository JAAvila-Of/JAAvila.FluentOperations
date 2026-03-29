using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary is not empty.
/// </summary>
internal class DictionaryNotBeEmptyValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain
) : IValidator, IRuleDescriptor
    where TKey : notnull
{
    public static DictionaryNotBeEmptyValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain
    ) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Dictionary.NotBeEmpty";
    string IRuleDescriptor.OperationName => "NotBeEmpty";
    Type IRuleDescriptor.SubjectType => typeof(IDictionary<,>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue().Count > 0)
        {
            return true;
        }

        ResultValidation =
            "The resulting dictionary was expected to be non-empty, but it was empty.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
