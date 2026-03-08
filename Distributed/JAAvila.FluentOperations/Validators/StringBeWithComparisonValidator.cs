using JAAvila.FluentOperations.Comparators;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string equals the expected value using the specified string comparison.
/// </summary>
internal class StringBeWithComparisonValidator(
    PrincipalChain<string?> chain,
    string? expectedValue,
    StringComparison comparison
) : IValidator
{
    public static StringBeWithComparisonValidator New(
        PrincipalChain<string?> chain,
        string? expectedValue,
        StringComparison comparison
    ) => new(chain, expectedValue, comparison);

    public string Expected { get; } = $"Be - <{comparison}>";
    public string ResultValidation { get; set; } = string.Empty;

    public bool Validate()
    {
        var value = chain.GetValue();

        // For Ordinal, delegate to the detailed StringComparator
        if (comparison == StringComparison.Ordinal)
        {
            var result = StringComparator.Compare(value, expectedValue);

            if (!result.Item1)
            {
                ResultValidation = result.Item2?.ToString() ?? string.Empty;
            }

            return result.Item1;
        }

        // For other comparisons, use string.Equals
        if (string.Equals(value, expectedValue, comparison))
        {
            return true;
        }

        ResultValidation =
            $"Expected the strings to be equal using {comparison}, but they differ.\n"
            + $"Expected: {FormatValue(expectedValue)}\n"
            + $"Actual:   {FormatValue(value)}";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }

    private static string FormatValue(string? value) => value is null ? "<null>" : $"\"{value}\"";
}
