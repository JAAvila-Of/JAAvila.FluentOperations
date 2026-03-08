using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is a finite number (not NaN or Infinity).
/// </summary>
internal class NullableDoubleBeFiniteValidator(PrincipalChain<double?> chain) : IValidator
{
    public static NullableDoubleBeFiniteValidator New(PrincipalChain<double?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (!double.IsNaN(value) && !double.IsInfinity(value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be finite, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
