using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string has the expected length.
/// </summary>
internal class StringHaveLengthValidator(PrincipalChain<string?> chain, int length) : IValidator
{
    public static StringHaveLengthValidator New(PrincipalChain<string?> chain, int length) =>
        new(chain, length);

    public string Expected => "(Length {0})";
    public string ResultValidation { get; set; }
    public string MessageKey => "String.HaveLength";

    public bool Validate()
    {
        if (chain.GetValue()!.Length == length)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have length {0}, but length {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
