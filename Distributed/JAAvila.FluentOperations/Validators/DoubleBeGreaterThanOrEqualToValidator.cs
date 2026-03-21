using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is greater than or equal to the expected value.
/// </summary>
internal class DoubleBeGreaterThanOrEqualToValidator(PrincipalChain<double> chain, double expected) : IValidator
{
    public static DoubleBeGreaterThanOrEqualToValidator New(PrincipalChain<double> chain, double expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Double.BeGreaterThanOrEqualTo";

    public bool Validate()
    {
        if (chain.GetValue() >= expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be greater than or equal to {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
