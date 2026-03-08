using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value has the expected number of decimal places.
/// </summary>
internal class NullableDecimalHavePrecisionValidator(
    PrincipalChain<decimal?> chain,
    int expectedDecimals
) : IValidator
{
    public static NullableDecimalHavePrecisionValidator New(
        PrincipalChain<decimal?> chain,
        int expectedDecimals
    ) => new(chain, expectedDecimals);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        var bits = decimal.GetBits(value);
        var scale = (bits[3] >> 16) & 0x7F;

        if (scale <= expectedDecimals)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have at most {0} decimal places, but it did not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
