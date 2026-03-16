using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the byte value does not equal the expected value.
/// </summary>
internal class ByteNotBeValidator(PrincipalChain<byte> chain, byte expected) : IValidator
{
    public static ByteNotBeValidator New(PrincipalChain<byte> chain, byte expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
