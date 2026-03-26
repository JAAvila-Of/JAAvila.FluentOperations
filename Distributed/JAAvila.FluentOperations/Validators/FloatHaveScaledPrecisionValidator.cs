using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value has at most <c>expectedDecimals</c> decimal places
/// and at most <c>maxTotalDigits</c> total significant digits.
/// </summary>
internal class FloatHaveScaledPrecisionValidator(
    PrincipalChain<float> chain,
    int expectedDecimals,
    int maxTotalDigits
) : IValidator, IRuleDescriptor
{
    public static FloatHaveScaledPrecisionValidator New(
        PrincipalChain<float> chain,
        int expectedDecimals,
        int maxTotalDigits
    ) => new(chain, expectedDecimals, maxTotalDigits);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Float.HaveScaledPrecision";
    string IRuleDescriptor.OperationName => "HaveScaledPrecision";
    Type IRuleDescriptor.SubjectType => typeof(float);
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
        var rounded = (float)Math.Round(value, expectedDecimals);
        if (Math.Abs(rounded - value) >= float.Epsilon)
        {
            ResultValidation =
                "The resulting value was expected to have at most {0} decimal places, but more decimal places were found.";
            return false;
        }

        // Total digits check using string representation for reliability
        var absValue = Math.Abs(value);
        var integerPart = (float)Math.Truncate(absValue);
        var integerDigits = integerPart == 0f ? 1 : (int)Math.Floor(Math.Log10(integerPart)) + 1;
        var strValue = value.ToString("G9");
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
