using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable boolean value does not equal the expected value.
/// </summary>
internal class NullableBooleanNotBeValidator(PrincipalChain<bool?> chain, bool? expectedValue)
    : IValidator
{
    public static NullableBooleanNotBeValidator New(
        PrincipalChain<bool?> chain,
        bool? expectedValue
    ) => new(chain, expectedValue);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() != expectedValue)
        {
            return true;
        }

        ResultValidation = "The resulting value should not be equal to {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
