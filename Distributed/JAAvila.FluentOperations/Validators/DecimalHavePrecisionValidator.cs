using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the decimal value has the expected number of decimal places.
/// </summary>
internal class DecimalHavePrecisionValidator(PrincipalChain<decimal> chain, int expectedDecimals) : IValidator
{
    public static DecimalHavePrecisionValidator New(PrincipalChain<decimal> chain, int expectedDecimals) =>
        new(chain, expectedDecimals);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();
        var bits = decimal.GetBits(value);
        var scale = (bits[3] >> 16) & 0x7F;

        if (scale <= expectedDecimals)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have at most {0} decimal places, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
