using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is entirely upper case.
/// </summary>
internal class StringBeUpperCasedValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeUpperCasedValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected => "Uppercase text.";
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeUpperCased";
    string IRuleDescriptor.OperationName => "BeUpperCased";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue().IsUpperCased())
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be uppercased.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
