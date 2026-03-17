using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ushort value equals the expected value.
/// </summary>
internal class UShortBeValidator(PrincipalChain<ushort> chain, ushort expected) : IValidator
{
    public static UShortBeValidator New(PrincipalChain<ushort> chain, ushort expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() == expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
