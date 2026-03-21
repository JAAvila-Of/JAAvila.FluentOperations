using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the decimal value is zero.
/// </summary>
internal class DecimalBeZeroValidator(PrincipalChain<decimal> chain) : IValidator
{
    public static DecimalBeZeroValidator New(PrincipalChain<decimal> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Decimal.BeZero";

    public bool Validate()
    {
        if (chain.GetValue() == 0m)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be zero, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
