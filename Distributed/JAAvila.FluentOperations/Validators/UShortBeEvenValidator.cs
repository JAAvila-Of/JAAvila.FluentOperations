using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ushort value is even.
/// </summary>
internal class UShortBeEvenValidator(PrincipalChain<ushort> chain) : IValidator
{
    public static UShortBeEvenValidator New(PrincipalChain<ushort> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() % 2 == 0)
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
