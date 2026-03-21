using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary is empty.
/// </summary>
internal class DictionaryBeEmptyValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain
) : IValidator where TKey : notnull
{
    public static DictionaryBeEmptyValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain
    ) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Dictionary.BeEmpty";

    public bool Validate()
    {
        if (chain.GetValue().Count == 0)
        {
            return true;
        }

        ResultValidation = "The resulting dictionary was expected to be empty, but it contained {0} entry(ies).";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
