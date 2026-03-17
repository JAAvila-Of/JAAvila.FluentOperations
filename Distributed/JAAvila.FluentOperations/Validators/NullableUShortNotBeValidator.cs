using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort value does not equal the expected value.
/// </summary>
internal class NullableUShortNotBeValidator(PrincipalChain<ushort?> chain, ushort? expected) : IValidator
{
    public static NullableUShortNotBeValidator New(PrincipalChain<ushort?> chain, ushort? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be {0}, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
