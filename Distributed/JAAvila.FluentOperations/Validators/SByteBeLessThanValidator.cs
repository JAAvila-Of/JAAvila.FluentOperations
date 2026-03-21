using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the sbyte value is less than the expected value.
/// </summary>
internal class SByteBeLessThanValidator(PrincipalChain<sbyte> chain, sbyte expected) : IValidator
{
    public static SByteBeLessThanValidator New(PrincipalChain<sbyte> chain, sbyte expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "SByte.BeLessThan";

    public bool Validate()
    {
        if (chain.GetValue() < expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be less than {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
