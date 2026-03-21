using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary reference equals the expected reference (reference equality).
/// </summary>
internal class DictionaryBeValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain,
    IDictionary<TKey, TValue> expected
) : IValidator
    where TKey : notnull
{
    public static DictionaryBeValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain,
        IDictionary<TKey, TValue> expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Dictionary.Be";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (ReferenceEquals(value, expected))
        {
            return true;
        }

        ResultValidation =
            "The resulting dictionary was expected to be the same reference as {0}, but a different reference was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
