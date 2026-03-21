using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is one of the specified allowed values.
/// </summary>
internal class NullableIntegerBeOneOfValidator(PrincipalChain<int?> chain, int[] values)
    : IValidator
{
    public static NullableIntegerBeOneOfValidator New(PrincipalChain<int?> chain, int[] values) =>
        new(chain, values);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableInteger.BeOneOf";

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
