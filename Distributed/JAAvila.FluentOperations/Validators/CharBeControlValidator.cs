using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a control character.
/// </summary>
internal class CharBeControlValidator(PrincipalChain<char> chain) : IValidator
{
    public static CharBeControlValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (char.IsControl(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a control character, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
