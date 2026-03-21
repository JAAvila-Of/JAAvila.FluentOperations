using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is within the specified tolerance of the expected value.
/// </summary>
internal class FloatBeApproximatelyValidator(
    PrincipalChain<float> chain,
    float expected,
    float tolerance
) : IValidator
{
    public static FloatBeApproximatelyValidator New(
        PrincipalChain<float> chain,
        float expected,
        float tolerance
    ) => new(chain, expected, tolerance);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Float.BeApproximately";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Math.Abs(value - expected) <= tolerance)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be approximately {0} (tolerance: {1}), but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
