using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the uint value does not equal the expected value.
/// </summary>
internal class UIntNotBeValidator(PrincipalChain<uint> chain, uint expected) : IValidator
{
    public static UIntNotBeValidator New(PrincipalChain<uint> chain, uint expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "UInt.NotBe";

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
