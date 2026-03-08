using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is a finite number (not NaN or Infinity).
/// </summary>
internal class DoubleBeFiniteValidator(PrincipalChain<double> chain) : IValidator
{
    public static DoubleBeFiniteValidator New(PrincipalChain<double> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!double.IsNaN(value) && !double.IsInfinity(value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be finite, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
