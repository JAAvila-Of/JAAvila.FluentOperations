using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ulong value is evenly divisible by the specified divisor.
/// </summary>
internal class ULongBeDivisibleByValidator(PrincipalChain<ulong> chain, ulong divisor) : IValidator
{
    public static ULongBeDivisibleByValidator New(PrincipalChain<ulong> chain, ulong divisor) =>
        new(chain, divisor);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() % divisor == 0UL)
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
