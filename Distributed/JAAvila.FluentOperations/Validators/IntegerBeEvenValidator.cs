using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the integer value is even.
/// </summary>
internal class IntegerBeEvenValidator(PrincipalChain<int> chain) : IValidator
{
    public static IntegerBeEvenValidator New(PrincipalChain<int> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() % 2 == 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be even, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
