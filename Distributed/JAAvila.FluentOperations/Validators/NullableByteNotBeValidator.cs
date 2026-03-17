using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable byte value does not equal the expected value.
/// </summary>
internal class NullableByteNotBeValidator(PrincipalChain<byte?> chain, byte? expected) : IValidator
{
    public static NullableByteNotBeValidator New(PrincipalChain<byte?> chain, byte? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be {0}, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
