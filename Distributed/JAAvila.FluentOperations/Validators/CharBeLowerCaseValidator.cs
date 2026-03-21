using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is a lowercase letter.
/// </summary>
internal class CharBeLowerCaseValidator(PrincipalChain<char> chain) : IValidator, IRuleDescriptor
{
    public static CharBeLowerCaseValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Char.BeLowerCase";
    string IRuleDescriptor.OperationName => "BeLowerCase";
    Type IRuleDescriptor.SubjectType => typeof(char);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (char.IsLower(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be a lowercase letter, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
