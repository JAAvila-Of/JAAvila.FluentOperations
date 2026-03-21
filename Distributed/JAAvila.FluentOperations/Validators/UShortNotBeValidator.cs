using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ushort value does not equal the expected value.
/// </summary>
internal class UShortNotBeValidator(PrincipalChain<ushort> chain, ushort expected) : IValidator
{
    public static UShortNotBeValidator New(PrincipalChain<ushort> chain, ushort expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "UShort.NotBe";

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
