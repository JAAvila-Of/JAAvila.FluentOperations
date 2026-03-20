using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string value is not one of the specified disallowed values.
/// </summary>
internal class StringNotBeOneOfValidator(
    PrincipalChain<string?> chain,
    string?[] expected,
    StringComparison? comparison
) : IValidator
{
    public static StringNotBeOneOfValidator New(
        PrincipalChain<string?> chain,
        string?[] expected,
        StringComparison? comparison = null
    ) => new(chain, expected, comparison);

    public string Expected { get; } = string.Join(", ", expected.Select(e => e is null ? "<null>" : $"\"{e}\""));
    public string ResultValidation { get; set; } = string.Empty;

    public bool Validate()
    {
        var value = chain.GetValue();
        var comp = comparison ?? StringComparison.Ordinal;

        if (!expected.Any(e => string.Equals(value, e, comp)))
        {
            return true;
        }

        ResultValidation = comparison.HasValue
            ? $"The resulting value was expected to not be one of [{{0}}] using {comp}, but {{1}} was found."
            : "The resulting value was expected to not be one of [{0}], but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
