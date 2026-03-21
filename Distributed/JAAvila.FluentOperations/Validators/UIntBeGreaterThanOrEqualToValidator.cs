using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the uint value is greater than or equal to the expected value.
/// </summary>
internal class UIntBeGreaterThanOrEqualToValidator(PrincipalChain<uint> chain, uint expected) : IValidator
{
    public static UIntBeGreaterThanOrEqualToValidator New(PrincipalChain<uint> chain, uint expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "UInt.BeGreaterThanOrEqualTo";

    public bool Validate()
    {
        if (chain.GetValue() >= expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be greater than or equal to {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
