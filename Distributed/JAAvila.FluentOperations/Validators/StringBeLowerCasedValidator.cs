using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is entirely lower case.
/// </summary>
internal class StringBeLowerCasedValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringBeLowerCasedValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected => "Uppercase text.";
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeLowerCased";

    public bool Validate()
    {
        if (chain.GetValue().IsLowerCased())
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be lowercased.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}