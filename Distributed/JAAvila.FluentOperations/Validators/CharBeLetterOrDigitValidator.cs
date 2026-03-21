using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a letter or digit.
/// </summary>
internal class CharBeLetterOrDigitValidator(PrincipalChain<char> chain) : IValidator, IRuleDescriptor
{
    public static CharBeLetterOrDigitValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Char.BeLetterOrDigit";
    string IRuleDescriptor.OperationName => "BeLetterOrDigit";
    Type IRuleDescriptor.SubjectType => typeof(char);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (char.IsLetterOrDigit(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a letter or digit, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
