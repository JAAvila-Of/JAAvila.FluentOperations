using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable short value is one of the specified allowed values.
/// </summary>
internal class NullableShortBeOneOfValidator(PrincipalChain<short?> chain, short[] values)
    : IValidator
{
    public static NullableShortBeOneOfValidator New(PrincipalChain<short?> chain, short[] values) =>
        new(chain, values);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
