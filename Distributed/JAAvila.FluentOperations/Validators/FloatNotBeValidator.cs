using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value does not equal the expected value.
/// </summary>
internal class FloatNotBeValidator(PrincipalChain<float> chain, float expected) : IValidator
{
    public static FloatNotBeValidator New(PrincipalChain<float> chain, float expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Float.NotBe";

    public bool Validate()
    {
        if (!chain.GetValue().Equals(expected))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
