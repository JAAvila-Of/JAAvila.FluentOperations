using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ulong value is odd.
/// </summary>
internal class ULongBeOddValidator(PrincipalChain<ulong> chain) : IValidator
{
    public static ULongBeOddValidator New(PrincipalChain<ulong> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "ULong.BeOdd";

    public bool Validate()
    {
        if (chain.GetValue() % 2UL != 0UL)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be odd, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
