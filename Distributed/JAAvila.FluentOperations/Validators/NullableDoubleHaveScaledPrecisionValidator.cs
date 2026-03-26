using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value has at most <c>expectedDecimals</c> decimal places
/// and at most <c>maxTotalDigits</c> total significant digits.
/// </summary>
internal class NullableDoubleHaveScaledPrecisionValidator(
    PrincipalChain<double?> chain,
    int expectedDecimals,
    int maxTotalDigits
) : IValidator, IRuleDescriptor
{
    public static NullableDoubleHaveScaledPrecisionValidator New(
        PrincipalChain<double?> chain,
        int expectedDecimals,
        int maxTotalDigits
    ) => new(chain, expectedDecimals, maxTotalDigits);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDouble.HaveScaledPrecision";
    string IRuleDescriptor.OperationName => "HaveScaledPrecision";
    Type IRuleDescriptor.SubjectType => typeof(double?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>
        {
            ["expectedDecimals"] = expectedDecimals,
            ["maxTotalDigits"] = maxTotalDigits,
        };

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        // Scale check
        var rounded = Math.Round(value, expectedDecimals);
        if (Math.Abs(rounded - value) >= double.Epsilon)
        {
            ResultValidation =
                "The resulting value was expected to have at most {0} decimal places, but it did not.";
            return false;
        }

        // Total digits check using string representation for reliability
        var absValue = Math.Abs(value);
        var integerPart = Math.Truncate(absValue);
        var integerDigits = integerPart == 0.0 ? 1 : (int)Math.Floor(Math.Log10(integerPart)) + 1;
        var strValue = value.ToString("G17");
        var dotIndex = strValue.IndexOf('.');
        var actualDecimals = dotIndex < 0 ? 0 : strValue.Length - dotIndex - 1;
        var totalDigits = integerDigits + Math.Min(actualDecimals, expectedDecimals);

        if (totalDigits > maxTotalDigits)
        {
            ResultValidation =
                "The resulting value was expected to have at most {0} total digits, but it did not.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
