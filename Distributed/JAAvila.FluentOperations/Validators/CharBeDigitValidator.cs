using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a digit character.
/// </summary>
internal class CharBeDigitValidator(PrincipalChain<char> chain) : IValidator
{
    public static CharBeDigitValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Char.BeDigit";

    public bool Validate()
    {
        if (char.IsDigit(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a digit, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
