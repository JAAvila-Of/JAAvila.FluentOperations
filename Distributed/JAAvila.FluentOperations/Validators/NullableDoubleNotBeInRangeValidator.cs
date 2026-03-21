using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is outside the specified inclusive range.
/// </summary>
internal class NullableDoubleNotBeInRangeValidator(
    PrincipalChain<double?> chain,
    double min,
    double max
) : IValidator
{
    public static NullableDoubleNotBeInRangeValidator New(
        PrincipalChain<double?> chain,
        double min,
        double max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDouble.NotBeInRange";

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
