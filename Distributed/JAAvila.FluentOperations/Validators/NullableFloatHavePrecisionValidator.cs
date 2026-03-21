using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value has the expected number of decimal places.
/// </summary>
internal class NullableFloatHavePrecisionValidator(
    PrincipalChain<float?> chain,
    int expectedDecimals
) : IValidator
{
    public static NullableFloatHavePrecisionValidator New(
        PrincipalChain<float?> chain,
        int expectedDecimals
    ) => new(chain, expectedDecimals);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableFloat.HavePrecision";

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        var rounded = (float)Math.Round(value, expectedDecimals);

        if (Math.Abs(rounded - value) < float.Epsilon)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have the given precision, but it did not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
