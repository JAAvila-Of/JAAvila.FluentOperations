using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the short value is strictly negative (less than zero).
/// </summary>
internal class ShortBeNegativeValidator(PrincipalChain<short> chain) : IValidator
{
    public static ShortBeNegativeValidator New(PrincipalChain<short> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
