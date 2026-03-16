using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint value does not equal the expected value.
/// </summary>
internal class NullableUIntNotBeValidator(PrincipalChain<uint?> chain, uint? expected) : IValidator
{
    public static NullableUIntNotBeValidator New(PrincipalChain<uint?> chain, uint? expected) =>
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
