using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the long value is evenly divisible by the specified divisor.
/// </summary>
internal class LongBeDivisibleByValidator(PrincipalChain<long> chain, long divisor) : IValidator
{
    public static LongBeDivisibleByValidator New(PrincipalChain<long> chain, long divisor) =>
        new(chain, divisor);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Long.BeDivisibleBy";

    public bool Validate()
    {
        if (chain.GetValue() % divisor == 0)
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
