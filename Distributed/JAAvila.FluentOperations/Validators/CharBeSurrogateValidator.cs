using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a surrogate character.
/// </summary>
internal class CharBeSurrogateValidator(PrincipalChain<char> chain) : IValidator
{
    public static CharBeSurrogateValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Char.BeSurrogate";

    public bool Validate()
    {
        if (char.IsSurrogate(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a surrogate character, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
