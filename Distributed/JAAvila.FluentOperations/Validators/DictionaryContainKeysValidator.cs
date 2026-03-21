using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the dictionary contains all of the specified keys.
/// </summary>
internal class DictionaryContainKeysValidator<TKey, TValue>(
    PrincipalChain<IDictionary<TKey, TValue>> chain,
    TKey[] keys
) : IValidator where TKey : notnull
{
    public static DictionaryContainKeysValidator<TKey, TValue> New(
        PrincipalChain<IDictionary<TKey, TValue>> chain,
        TKey[] keys
    ) => new(chain, keys);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Dictionary.ContainKeys";

    /// <summary>
    /// The keys that were not found in the dictionary. Available after <see cref="Validate"/> returns false.
    /// </summary>
    public IReadOnlyList<TKey> MissingKeys { get; private set; } = [];

    public bool Validate()
    {
        var dict = chain.GetValue();
        var missing = keys.Where(k => !dict.ContainsKey(k)).ToList();

        if (missing.Count == 0)
        {
            return true;
        }

        MissingKeys = missing;
        ResultValidation =
            "The resulting dictionary was expected to contain keys {0}, but the following keys were missing: {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
