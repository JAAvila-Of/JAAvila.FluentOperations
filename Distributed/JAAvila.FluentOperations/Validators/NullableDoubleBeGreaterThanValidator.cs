using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is greater than the expected value.
/// </summary>
internal class NullableDoubleBeGreaterThanValidator(
    PrincipalChain<double?> chain,
    double comparison
) : IValidator
{
    public static NullableDoubleBeGreaterThanValidator New(
        PrincipalChain<double?> chain,
        double comparison
    ) => new(chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDouble.BeGreaterThan";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value > comparison)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be greater than the given value, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
