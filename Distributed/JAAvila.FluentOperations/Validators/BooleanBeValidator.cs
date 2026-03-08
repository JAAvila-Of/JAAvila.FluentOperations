using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the boolean value equals the expected value.
/// </summary>
internal class BooleanBeValidator(PrincipalChain<bool> chain, bool? expectedValue) : IValidator
{
    public static BooleanBeValidator New(PrincipalChain<bool> chain, bool? expectedValue) =>
        new(chain, expectedValue);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() == expectedValue)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
