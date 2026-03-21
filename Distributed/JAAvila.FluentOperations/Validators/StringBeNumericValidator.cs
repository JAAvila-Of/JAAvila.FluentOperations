using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string contains only numeric characters.
/// </summary>
internal class StringBeNumericValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringBeNumericValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeNumeric";

    public bool Validate()
    {
        if (chain.GetValue()!.All(char.IsDigit))
        {
            return true;
        }

        ResultValidation = "The value was expected to contain only numeric digits (0-9).";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
