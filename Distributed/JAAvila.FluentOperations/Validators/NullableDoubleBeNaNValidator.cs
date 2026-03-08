using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is NaN.
/// </summary>
internal class NullableDoubleBeNaNValidator(PrincipalChain<double?> chain) : IValidator
{
    public static NullableDoubleBeNaNValidator New(PrincipalChain<double?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (double.IsNaN(value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be NaN, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
