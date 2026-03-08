using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the integer value is greater than or equal to the expected value.
/// </summary>
internal class IntegerBeGreaterThanOrEqualToValidator(PrincipalChain<int> chain, int expected) : IValidator
{
    public static IntegerBeGreaterThanOrEqualToValidator New(PrincipalChain<int> chain, int expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() >= expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be greater than or equal to {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
