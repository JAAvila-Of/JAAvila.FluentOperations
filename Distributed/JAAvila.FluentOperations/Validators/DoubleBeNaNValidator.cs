using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is NaN.
/// </summary>
internal class DoubleBeNaNValidator(PrincipalChain<double> chain) : IValidator
{
    public static DoubleBeNaNValidator New(PrincipalChain<double> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (double.IsNaN(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be NaN, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
