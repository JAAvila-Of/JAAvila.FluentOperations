using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string contains only alphanumeric characters.
/// </summary>
internal class StringBeAlphanumericValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringBeAlphanumericValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeAlphanumeric";

    public bool Validate()
    {
        if (chain.GetValue()!.All(char.IsLetterOrDigit))
        {
            return true;
        }

        ResultValidation = "The value was expected to contain only alphanumeric characters.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
