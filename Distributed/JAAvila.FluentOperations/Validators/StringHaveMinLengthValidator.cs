using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string has at least the expected length.
/// </summary>
internal class StringHaveMinLengthValidator(PrincipalChain<string?> chain, int minLength)
    : IValidator
{
    public static StringHaveMinLengthValidator New(PrincipalChain<string?> chain, int minLength) =>
        new(chain, minLength);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue()!.Length >= minLength)
        {
            return true;
        }

        ResultValidation =
            "The value was expected to have a minimum length of {0}, but the actual length was {1}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
