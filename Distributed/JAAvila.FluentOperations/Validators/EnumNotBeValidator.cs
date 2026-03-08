using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the enum value does not equal the expected value.
/// </summary>
internal class EnumNotBeValidator<T>(PrincipalChain<T> chain, T expected) : IValidator
    where T : Enum
{
    public static EnumNotBeValidator<T> New(PrincipalChain<T> chain, T expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!chain.GetValue().Equals(expected))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
