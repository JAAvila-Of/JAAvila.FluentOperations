using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable byte value is one of the specified allowed values.
/// </summary>
internal class NullableByteBeOneOfValidator(PrincipalChain<byte?> chain, byte[] values)
    : IValidator
{
    public static NullableByteBeOneOfValidator New(PrincipalChain<byte?> chain, byte[] values) =>
        new(chain, values);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableByte.BeOneOf";

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (values.Contains(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be one of the given values, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
