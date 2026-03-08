using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is NaN.
/// </summary>
internal class NullableFloatBeNaNValidator(PrincipalChain<float?> chain) : IValidator
{
    public static NullableFloatBeNaNValidator New(PrincipalChain<float?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (float.IsNaN(value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be NaN, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
