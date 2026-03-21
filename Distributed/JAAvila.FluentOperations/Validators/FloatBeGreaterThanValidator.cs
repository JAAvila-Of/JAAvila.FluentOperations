using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is greater than the expected value.
/// </summary>
internal class FloatBeGreaterThanValidator(PrincipalChain<float> chain, float expected) : IValidator
{
    public static FloatBeGreaterThanValidator New(PrincipalChain<float> chain, float expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Float.BeGreaterThan";

    public bool Validate()
    {
        if (chain.GetValue() > expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be greater than {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
