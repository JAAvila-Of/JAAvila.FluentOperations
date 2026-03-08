using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the long value is zero.
/// </summary>
internal class LongBeZeroValidator(PrincipalChain<long> chain) : IValidator
{
    public static LongBeZeroValidator New(PrincipalChain<long> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() == 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be zero, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
