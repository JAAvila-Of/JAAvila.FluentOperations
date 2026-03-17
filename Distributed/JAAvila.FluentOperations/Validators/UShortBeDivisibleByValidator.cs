using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ushort value is evenly divisible by the specified divisor.
/// </summary>
internal class UShortBeDivisibleByValidator(PrincipalChain<ushort> chain, ushort divisor) : IValidator
{
    public static UShortBeDivisibleByValidator New(PrincipalChain<ushort> chain, ushort divisor) =>
        new(chain, divisor);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
