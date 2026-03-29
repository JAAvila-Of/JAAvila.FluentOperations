using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the decimal value has at most <c>expectedDecimals</c> decimal places
/// and at most <c>maxTotalDigits</c> total significant digits.
/// </summary>
internal class DecimalHaveScaledPrecisionValidator(
    PrincipalChain<decimal> chain,
    int expectedDecimals,
    int maxTotalDigits
) : IValidator, IRuleDescriptor
{
    public static DecimalHaveScaledPrecisionValidator New(
        PrincipalChain<decimal> chain,
        int expectedDecimals,
        int maxTotalDigits
    ) => new(chain, expectedDecimals, maxTotalDigits);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Decimal.HaveScaledPrecision";
    string IRuleDescriptor.OperationName => "HaveScaledPrecision";
    Type IRuleDescriptor.SubjectType => typeof(decimal);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>
        {
            ["expectedDecimals"] = expectedDecimals,
            ["maxTotalDigits"] = maxTotalDigits,
        };

    public bool Validate()
    {
        var value = chain.GetValue();
        var bits = decimal.GetBits(value);
        var scale = (bits[3] >> 16) & 0x7F;

        if (scale > expectedDecimals)
        {
            ResultValidation =
                "The resulting value was expected to have at most {0} decimal places, but {1} decimal places were found.";
            return false;
        }

        var absValue = Math.Abs(value);
        var integerPart = decimal.Truncate(absValue);
        var integerDigits =
            integerPart == 0m ? 1 : (int)Math.Floor(Math.Log10((double)integerPart)) + 1;
        var totalDigits = integerDigits + scale;

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
