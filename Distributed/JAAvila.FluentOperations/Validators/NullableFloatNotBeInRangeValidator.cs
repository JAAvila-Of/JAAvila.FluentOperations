using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is outside the specified inclusive range.
/// </summary>
internal class NullableFloatNotBeInRangeValidator(
    PrincipalChain<float?> chain,
    float min,
    float max
) : IValidator
{
    public static NullableFloatNotBeInRangeValidator New(
        PrincipalChain<float?> chain,
        float min,
        float max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < min || value > max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected not to be in the given range, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
