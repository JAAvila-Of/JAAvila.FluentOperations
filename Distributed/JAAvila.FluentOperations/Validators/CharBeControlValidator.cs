using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a control character.
/// </summary>
internal class CharBeControlValidator(PrincipalChain<char> chain) : IValidator, IRuleDescriptor
{
    public static CharBeControlValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Char.BeControl";
    string IRuleDescriptor.OperationName => "BeControl";
    Type IRuleDescriptor.SubjectType => typeof(char);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

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
