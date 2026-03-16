using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable char value is one of the specified allowed values.
/// </summary>
internal class NullableCharBeOneOfValidator(PrincipalChain<char?> chain, char[] values)
    : IValidator
{
    public static NullableCharBeOneOfValidator New(PrincipalChain<char?> chain, char[] values) =>
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
