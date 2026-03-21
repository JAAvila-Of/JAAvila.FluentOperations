using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value has the expected number of decimal places.
/// </summary>
internal class FloatHavePrecisionValidator(PrincipalChain<float> chain, int expectedDecimals)
    : IValidator
{
    public static FloatHavePrecisionValidator New(
        PrincipalChain<float> chain,
        int expectedDecimals
    ) => new(chain, expectedDecimals);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Float.HavePrecision";

    public bool Validate()
    {
        var value = chain.GetValue();
        var rounded = (float)Math.Round(value, expectedDecimals);

        if (Math.Abs(rounded - value) < float.Epsilon)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have at most {0} decimal places, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
