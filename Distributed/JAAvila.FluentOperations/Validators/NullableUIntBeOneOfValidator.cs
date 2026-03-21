using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint value is one of the specified allowed values.
/// </summary>
internal class NullableUIntBeOneOfValidator(PrincipalChain<uint?> chain, uint[] values)
    : IValidator
{
    public static NullableUIntBeOneOfValidator New(PrincipalChain<uint?> chain, uint[] values) =>
        new(chain, values);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUInt.BeOneOf";

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
