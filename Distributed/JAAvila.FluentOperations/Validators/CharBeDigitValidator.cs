using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a digit character.
/// </summary>
internal class CharBeDigitValidator(PrincipalChain<char> chain) : IValidator, IRuleDescriptor
{
    public static CharBeDigitValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Char.BeDigit";
    string IRuleDescriptor.OperationName => "BeDigit";
    Type IRuleDescriptor.SubjectType => typeof(char);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

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
