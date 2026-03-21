using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable short value is not one of the specified disallowed values.
/// </summary>
internal class NullableShortNotBeOneOfValidator(PrincipalChain<short?> chain, short[] values)
    : IValidator
{
    public static NullableShortNotBeOneOfValidator New(
        PrincipalChain<short?> chain,
        short[] values
    ) => new(chain, values);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableShort.NotBeOneOf";

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
