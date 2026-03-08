using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is greater than the expected value.
/// </summary>
internal class DoubleBeGreaterThanValidator(PrincipalChain<double> chain, double expected) : IValidator
{
    public static DoubleBeGreaterThanValidator New(PrincipalChain<double> chain, double expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() > expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be greater than {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
