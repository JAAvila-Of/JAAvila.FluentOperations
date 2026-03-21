using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the decimal value equals itself when rounded to the specified decimal places.
/// </summary>
internal class DecimalBeRoundedToValidator(PrincipalChain<decimal> chain, int decimals) : IValidator
{
    public static DecimalBeRoundedToValidator New(PrincipalChain<decimal> chain, int decimals) =>
        new(chain, decimals);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Decimal.BeRoundedTo";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Math.Round(value, decimals) == value)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be rounded to {0} decimal places, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
