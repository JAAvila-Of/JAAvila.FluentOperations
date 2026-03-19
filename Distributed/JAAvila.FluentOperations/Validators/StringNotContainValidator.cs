using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string does not contain the expected substring.
/// </summary>
internal class StringNotContainValidator(
    string expected,
    PrincipalChain<string?> chain,
    StringComparison comparison = StringComparison.Ordinal
) : IValidator
{
    public static StringNotContainValidator New(string expected, PrincipalChain<string?> chain) =>
        new(expected, chain);

    public static StringNotContainValidator New(
        string expected,
        PrincipalChain<string?> chain,
        StringComparison comparison) =>
        new(expected, chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!chain.GetValue()!.Contains(expected, comparison))
        {
            return true;
        }

        ResultValidation = "The value was expected to not contain {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
