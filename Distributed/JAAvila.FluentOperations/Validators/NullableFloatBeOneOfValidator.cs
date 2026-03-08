using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is one of the specified allowed values.
/// </summary>
internal class NullableFloatBeOneOfValidator(PrincipalChain<float?> chain, float[] expected)
    : IValidator
{
    public static NullableFloatBeOneOfValidator New(
        PrincipalChain<float?> chain,
        float[] expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (expected.Contains(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be one of the given values, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
