using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the integer value is odd.
/// </summary>
internal class IntegerBeOddValidator(PrincipalChain<int> chain) : IValidator
{
    public static IntegerBeOddValidator New(PrincipalChain<int> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() % 2 != 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be odd, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
