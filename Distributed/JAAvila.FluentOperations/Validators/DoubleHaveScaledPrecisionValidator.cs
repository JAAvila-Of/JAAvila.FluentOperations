using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value has at most <c>expectedDecimals</c> decimal places
/// and at most <c>maxTotalDigits</c> total significant digits.
/// </summary>
internal class DoubleHaveScaledPrecisionValidator(
    PrincipalChain<double> chain,
    int expectedDecimals,
    int maxTotalDigits
) : IValidator, IRuleDescriptor
{
    public static DoubleHaveScaledPrecisionValidator New(
        PrincipalChain<double> chain,
        int expectedDecimals,
        int maxTotalDigits
    ) => new(chain, expectedDecimals, maxTotalDigits);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Double.HaveScaledPrecision";
    string IRuleDescriptor.OperationName => "HaveScaledPrecision";
    Type IRuleDescriptor.SubjectType => typeof(double);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>
        {
            ["expectedDecimals"] = expectedDecimals,
            ["maxTotalDigits"] = maxTotalDigits,
        };

    public bool Validate()
    {
        var value = chain.GetValue();

        // Scale check: ensure value rounds cleanly to expectedDecimals places
        var rounded = Math.Round(value, expectedDecimals);
        if (Math.Abs(rounded - value) >= double.Epsilon)
        {
            ResultValidation =
                "The resulting value was expected to have at most {0} decimal places, but more decimal places were found.";
            return false;
        }

        // Total digits check: count integer digits + decimal places
        var absValue = Math.Abs(value);
        var integerPart = Math.Truncate(absValue);
        var integerDigits = integerPart == 0.0 ? 1 : (int)Math.Floor(Math.Log10(integerPart)) + 1;
        var actualScale = (int)Math.Round(
            -Math.Log10(Math.Abs(value - Math.Truncate(value)) + double.Epsilon)
        );
        // Use the string representation to reliably count decimal places
        var strValue = value.ToString("G17");
        var dotIndex = strValue.IndexOf('.');
        var actualDecimals = dotIndex < 0 ? 0 : strValue.Length - dotIndex - 1;
        var totalDigits = integerDigits + Math.Min(actualDecimals, expectedDecimals);

        if (totalDigits > maxTotalDigits)
        {
            ResultValidation =
                "The resulting value was expected to have at most {0} total digits, but {1} total digits were found.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
