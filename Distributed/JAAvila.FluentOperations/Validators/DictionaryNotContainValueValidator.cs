using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary does not contain the expected value.
/// </summary>
internal class DictionaryNotContainValueValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain,
    TValue value
) : IValidator where TKey : notnull
{
    public static DictionaryNotContainValueValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain,
        TValue value
    ) => new(chain, value);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!chain.GetValue().Values.Contains(value))
        {
            return true;
        }

        ResultValidation = "The resulting dictionary was expected to not contain value {0}, but it did.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
