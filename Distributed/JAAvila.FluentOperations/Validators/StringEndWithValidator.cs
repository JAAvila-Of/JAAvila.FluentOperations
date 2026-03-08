using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string ends with the expected substring.
/// </summary>
internal class StringEndWithValidator(
    string suffix,
    PrincipalChain<string?> chain,
    StringComparison comparison = StringComparison.Ordinal
) : IValidator
{
    public static StringEndWithValidator New(string suffix, PrincipalChain<string?> chain) =>
        new(suffix, chain);

    public static StringEndWithValidator New(
        string suffix,
        PrincipalChain<string?> chain,
        StringComparison comparison
    ) => new(suffix, chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue()!.EndsWith(suffix, comparison))
        {
            return true;
        }

        ResultValidation = "The value was expected to end with \"{0}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
