using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is positive infinity.
/// </summary>
internal class NullableFloatBePositiveInfinityValidator(PrincipalChain<float?> chain) : IValidator
{
    public static NullableFloatBePositiveInfinityValidator New(PrincipalChain<float?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (float.IsPositiveInfinity(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be positive infinity, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
