using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ushort value is greater than the expected value.
/// </summary>
internal class UShortBeGreaterThanValidator(PrincipalChain<ushort> chain, ushort expected) : IValidator
{
    public static UShortBeGreaterThanValidator New(PrincipalChain<ushort> chain, ushort expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "UShort.BeGreaterThan";

    public bool Validate()
    {
        if (chain.GetValue() > expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be greater than {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
