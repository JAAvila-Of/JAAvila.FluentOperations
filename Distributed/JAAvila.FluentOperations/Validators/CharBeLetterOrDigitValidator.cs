using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a letter or digit.
/// </summary>
internal class CharBeLetterOrDigitValidator(PrincipalChain<char> chain) : IValidator
{
    public static CharBeLetterOrDigitValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Char.BeLetterOrDigit";

    public bool Validate()
    {
        if (char.IsLetterOrDigit(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a letter or digit, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
