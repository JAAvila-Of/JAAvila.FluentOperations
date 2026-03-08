using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string contains only alphabetic characters.
/// </summary>
internal class StringBeAlphabeticValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringBeAlphabeticValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue()!.All(char.IsLetter))
        {
            return true;
        }

        ResultValidation = "The value was expected to contain only alphabetic characters.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
