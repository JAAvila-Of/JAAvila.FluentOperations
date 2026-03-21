using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ulong value is zero.
/// </summary>
internal class ULongBeZeroValidator(PrincipalChain<ulong> chain) : IValidator
{
    public static ULongBeZeroValidator New(PrincipalChain<ulong> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "ULong.BeZero";

    public bool Validate()
    {
        if (chain.GetValue() == 0UL)
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
