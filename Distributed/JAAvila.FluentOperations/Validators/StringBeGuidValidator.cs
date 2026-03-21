using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string value is a valid GUID.
/// </summary>
internal class StringBeGuidValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringBeGuidValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeGuid";

    public bool Validate()
    {
        if (Guid.TryParse(chain.GetValue()!, out _))
        {
            return true;
        }

        ResultValidation = "The value was expected to be a valid GUID.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
