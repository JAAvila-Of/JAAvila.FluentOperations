using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary is not empty.
/// </summary>
internal class DictionaryNotBeEmptyValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain
) : IValidator where TKey : notnull
{
    public static DictionaryNotBeEmptyValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain
    ) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue().Count > 0)
        {
            return true;
        }

        ResultValidation = "The resulting dictionary was expected to be non-empty, but it was empty.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
