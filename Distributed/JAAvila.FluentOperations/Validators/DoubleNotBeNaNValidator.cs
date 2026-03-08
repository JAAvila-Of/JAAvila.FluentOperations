using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is not NaN.
/// </summary>
internal class DoubleNotBeNaNValidator(PrincipalChain<double> chain) : IValidator
{
    public static DoubleNotBeNaNValidator New(PrincipalChain<double> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!double.IsNaN(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be NaN, but NaN was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
