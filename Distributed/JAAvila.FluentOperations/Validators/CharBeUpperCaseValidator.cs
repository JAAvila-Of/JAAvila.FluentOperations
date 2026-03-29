using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is an uppercase letter.
/// </summary>
internal class CharBeUpperCaseValidator(PrincipalChain<char> chain) : IValidator, IRuleDescriptor
{
    public static CharBeUpperCaseValidator New(PrincipalChain<char> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Char.BeUpperCase";
    string IRuleDescriptor.OperationName => "BeUpperCase";
    Type IRuleDescriptor.SubjectType => typeof(char);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (char.IsUpper(chain.GetValue()))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be an uppercase letter, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
