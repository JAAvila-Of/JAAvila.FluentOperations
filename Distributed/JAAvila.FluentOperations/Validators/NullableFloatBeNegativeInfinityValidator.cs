using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is negative infinity.
/// </summary>
internal class NullableFloatBeNegativeInfinityValidator(PrincipalChain<float?> chain) : IValidator
{
    public static NullableFloatBeNegativeInfinityValidator New(PrincipalChain<float?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableFloat.BeNegativeInfinity";

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (float.IsNegativeInfinity(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be negative infinity, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
