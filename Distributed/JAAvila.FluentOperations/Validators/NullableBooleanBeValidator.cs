using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable boolean value equals the expected value.
/// </summary>
internal class NullableBooleanBeValidator(PrincipalChain<bool?> chain, bool? expectedValue) : IValidator
{
    public static NullableBooleanBeValidator New(PrincipalChain<bool?> chain, bool? expectedValue) =>
        new(chain, expectedValue);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableBoolean.Be";

    public bool Validate()
    {
        if (chain.GetValue() == expectedValue)
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