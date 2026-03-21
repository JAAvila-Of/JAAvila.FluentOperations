using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is a finite number (not NaN or Infinity).
/// </summary>
internal class FloatBeFiniteValidator(PrincipalChain<float> chain) : IValidator
{
    public static FloatBeFiniteValidator New(PrincipalChain<float> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Float.BeFinite";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!float.IsNaN(value) && !float.IsInfinity(value))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be finite, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
