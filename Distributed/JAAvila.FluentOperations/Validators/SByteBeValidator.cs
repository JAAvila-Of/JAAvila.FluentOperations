using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the sbyte value equals the expected value.
/// </summary>
internal class SByteBeValidator(PrincipalChain<sbyte> chain, sbyte expected) : IValidator
{
    public static SByteBeValidator New(PrincipalChain<sbyte> chain, sbyte expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "SByte.Be";

    public bool Validate()
    {
        if (chain.GetValue() == expected)
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
