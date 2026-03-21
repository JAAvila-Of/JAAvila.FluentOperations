using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the decimal value is odd.
/// </summary>
internal class DecimalBeOddValidator(PrincipalChain<decimal> chain) : IValidator
{
    public static DecimalBeOddValidator New(PrincipalChain<decimal> principalChain) =>
        new(principalChain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Decimal.BeOdd";

    public bool Validate()
    {
        if (chain.GetValue() % 2m != 0m)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be odd, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
