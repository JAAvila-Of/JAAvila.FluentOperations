using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ulong value is even.
/// </summary>
internal class ULongBeEvenValidator(PrincipalChain<ulong> chain) : IValidator
{
    public static ULongBeEvenValidator New(PrincipalChain<ulong> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() % 2UL == 0UL)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be even, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
