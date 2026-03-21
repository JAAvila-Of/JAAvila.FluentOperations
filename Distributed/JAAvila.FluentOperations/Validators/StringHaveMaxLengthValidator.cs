using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string has at most the expected length.
/// </summary>
internal class StringHaveMaxLengthValidator(PrincipalChain<string?> chain, int maxLength)
    : IValidator
{
    public static StringHaveMaxLengthValidator New(PrincipalChain<string?> chain, int maxLength) =>
        new(chain, maxLength);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.HaveMaxLength";

    public bool Validate()
    {
        if (chain.GetValue()!.Length <= maxLength)
        {
            return true;
        }

        ResultValidation =
            "The value was expected to have a maximum length of {0}, but the actual length was {1}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
