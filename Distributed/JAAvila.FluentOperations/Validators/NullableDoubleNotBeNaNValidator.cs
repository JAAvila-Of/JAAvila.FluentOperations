using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is not NaN.
/// </summary>
internal class NullableDoubleNotBeNaNValidator(PrincipalChain<double?> chain) : IValidator
{
    public static NullableDoubleNotBeNaNValidator New(PrincipalChain<double?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (!double.IsNaN(value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be NaN, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
