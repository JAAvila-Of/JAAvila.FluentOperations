using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a white-space character.
/// </summary>
internal class CharBeWhiteSpaceValidator(PrincipalChain<char> chain) : IValidator, IRuleDescriptor
{
    public static CharBeWhiteSpaceValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Char.BeWhiteSpace";
    string IRuleDescriptor.OperationName => "BeWhiteSpace";
    Type IRuleDescriptor.SubjectType => typeof(char);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (char.IsWhiteSpace(chain.GetValue()))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be a white-space character, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
