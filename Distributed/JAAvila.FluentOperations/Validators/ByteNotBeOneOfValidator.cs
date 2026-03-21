using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the byte value is not one of the specified disallowed values.
/// </summary>
internal class ByteNotBeOneOfValidator(PrincipalChain<byte> chain, params byte[] expected) : IValidator
{
    public static ByteNotBeOneOfValidator New(PrincipalChain<byte> chain, params byte[] expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Byte.NotBeOneOf";

    public bool Validate()
    {
        if (!expected.Contains(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be one of [{0}], but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
