using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable long value does not equal the expected value.
/// </summary>
internal class NullableLongNotBeValidator(PrincipalChain<long?> chain, long? expected) : IValidator
{
    public static NullableLongNotBeValidator New(PrincipalChain<long?> chain, long? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableLong.NotBe";

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
