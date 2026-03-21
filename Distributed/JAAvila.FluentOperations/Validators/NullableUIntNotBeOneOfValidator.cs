using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint value is not one of the specified disallowed values.
/// </summary>
internal class NullableUIntNotBeOneOfValidator(PrincipalChain<uint?> chain, uint[] values)
    : IValidator
{
    public static NullableUIntNotBeOneOfValidator New(
        PrincipalChain<uint?> chain,
        uint[] values
    ) => new(chain, values);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUInt.NotBeOneOf";

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (!values.Contains(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected not to be one of the given values, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
