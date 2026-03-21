using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is not one of the specified disallowed values.
/// </summary>
internal class NullableIntegerNotBeOneOfValidator(PrincipalChain<int?> chain, int[] values)
    : IValidator
{
    public static NullableIntegerNotBeOneOfValidator New(
        PrincipalChain<int?> chain,
        int[] values
    ) => new(chain, values);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableInteger.NotBeOneOf";

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
