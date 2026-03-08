using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the decimal value is evenly divisible by the specified divisor.
/// </summary>
internal class DecimalBeDivisibleByValidator(PrincipalChain<decimal> chain, decimal divisor) : IValidator
{
    public static DecimalBeDivisibleByValidator New(PrincipalChain<decimal> chain, decimal divisor) =>
        new(chain, divisor);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() % divisor == 0m)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be divisible by {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
