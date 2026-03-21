using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a punctuation character.
/// </summary>
internal class CharBePunctuationValidator(PrincipalChain<char> chain) : IValidator
{
    public static CharBePunctuationValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Char.BePunctuation";

    public bool Validate()
    {
        if (char.IsPunctuation(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a punctuation character, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
