using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the enum value equals the expected value.
/// </summary>
internal class EnumBeValidator<T>(PrincipalChain<T> chain, T expected) : IValidator
    where T : Enum
{
    public static EnumBeValidator<T> New(PrincipalChain<T> chain, T expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Enum.Be";

    public bool Validate()
    {
        if (chain.GetValue().Equals(expected))
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
