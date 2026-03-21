using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ushort value is strictly positive (greater than zero).
/// </summary>
internal class UShortBePositiveValidator(PrincipalChain<ushort> chain) : IValidator
{
    public static UShortBePositiveValidator New(PrincipalChain<ushort> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "UShort.BePositive";

    public bool Validate()
    {
        if (chain.GetValue() > 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be positive, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
