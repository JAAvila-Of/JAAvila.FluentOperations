using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value equals itself when rounded to the specified decimal places.
/// </summary>
internal class DoubleBeRoundedToValidator(PrincipalChain<double> chain, int decimals) : IValidator
{
    public static DoubleBeRoundedToValidator New(PrincipalChain<double> chain, int decimals) =>
        new(chain, decimals);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Double.BeRoundedTo";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Math.Abs(Math.Round(value, decimals) - value) < 1e-10)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be rounded to {0} decimal places, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
