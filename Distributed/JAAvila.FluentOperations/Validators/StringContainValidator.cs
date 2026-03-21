using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string contains the expected element.
/// </summary>
internal class StringContainValidator(
    string expected,
    PrincipalChain<string?> chain,
    StringComparison comparison = StringComparison.Ordinal
) : IValidator
{
    public static StringContainValidator New(string expected, PrincipalChain<string?> chain) =>
        new(expected, chain);

    public static StringContainValidator New(
        string expected,
        PrincipalChain<string?> chain,
        StringComparison comparison) =>
        new(expected, chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.Contain";

    public bool Validate()
    {
        if (chain.GetValue()!.Contains(expected, comparison))
        {
            return true;
        }

        ResultValidation = "The value was expected to contain {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
