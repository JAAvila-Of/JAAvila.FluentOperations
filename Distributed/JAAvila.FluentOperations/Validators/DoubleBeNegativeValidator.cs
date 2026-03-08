using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is strictly negative.
/// </summary>
internal class DoubleBeNegativeValidator(PrincipalChain<double> chain) : IValidator
{
    public static DoubleBeNegativeValidator New(PrincipalChain<double> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() < 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be negative, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
