using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is strictly negative.
/// </summary>
internal class FloatBeNegativeValidator(PrincipalChain<float> chain) : IValidator
{
    public static FloatBeNegativeValidator New(PrincipalChain<float> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Float.BeNegative";

    public bool Validate()
    {
        if (chain.GetValue() < 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be negative, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
