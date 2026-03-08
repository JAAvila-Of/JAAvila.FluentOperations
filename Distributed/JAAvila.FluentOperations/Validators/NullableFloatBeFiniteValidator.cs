using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is a finite number (not NaN or Infinity).
/// </summary>
internal class NullableFloatBeFiniteValidator(PrincipalChain<float?> chain) : IValidator
{
    public static NullableFloatBeFiniteValidator New(PrincipalChain<float?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (!float.IsNaN(value) && !float.IsInfinity(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be a finite number, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
