using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ulong value is not one of the specified disallowed values.
/// </summary>
internal class NullableULongNotBeOneOfValidator(PrincipalChain<ulong?> chain, ulong[] values)
    : IValidator
{
    public static NullableULongNotBeOneOfValidator New(
        PrincipalChain<ulong?> chain,
        ulong[] values
    ) => new(chain, values);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
