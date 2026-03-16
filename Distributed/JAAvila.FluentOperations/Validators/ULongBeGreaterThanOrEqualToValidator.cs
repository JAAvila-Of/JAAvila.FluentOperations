using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ulong value is greater than or equal to the expected value.
/// </summary>
internal class ULongBeGreaterThanOrEqualToValidator(PrincipalChain<ulong> chain, ulong expected) : IValidator
{
    public static ULongBeGreaterThanOrEqualToValidator New(PrincipalChain<ulong> chain, ulong expected) =>
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
