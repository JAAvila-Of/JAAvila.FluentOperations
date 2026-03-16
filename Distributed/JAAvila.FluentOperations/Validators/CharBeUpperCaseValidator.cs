using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is an uppercase letter.
/// </summary>
internal class CharBeUpperCaseValidator(PrincipalChain<char> chain) : IValidator
{
    public static CharBeUpperCaseValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (char.IsUpper(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be an uppercase letter, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
