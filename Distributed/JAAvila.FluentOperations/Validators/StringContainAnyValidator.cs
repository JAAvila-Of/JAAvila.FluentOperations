using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string contains at least one of the expected substrings.
/// </summary>
internal class StringContainAnyValidator(
    PrincipalChain<string?> chain,
    string[] substrings,
    StringComparison? comparison
) : IValidator
{
    public static StringContainAnyValidator New(
        PrincipalChain<string?> chain,
        string[] substrings,
        StringComparison? comparison = null
    ) => new(chain, substrings, comparison);

    public string Expected => string.Join(", ", substrings);
    public string ResultValidation { get; set; }
    public string MessageKey => "String.ContainAny";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (substrings.Any(s => value!.Contains(s, comparison ?? StringComparison.Ordinal)))
        {
            return true;
        }

        ResultValidation =
            "The value was expected to contain at least one of the specified substrings, but none were found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
