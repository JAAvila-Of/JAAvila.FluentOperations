using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is negative infinity.
/// </summary>
internal class FloatBeNegativeInfinityValidator(PrincipalChain<float> chain) : IValidator
{
    public static FloatBeNegativeInfinityValidator New(PrincipalChain<float> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Float.BeNegativeInfinity";

    public bool Validate()
    {
        if (float.IsNegativeInfinity(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be negative infinity, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
