using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a letter.
/// </summary>
internal class CharBeLetterValidator(PrincipalChain<char> chain) : IValidator
{
    public static CharBeLetterValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Char.BeLetter";

    public bool Validate()
    {
        if (char.IsLetter(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a letter, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
