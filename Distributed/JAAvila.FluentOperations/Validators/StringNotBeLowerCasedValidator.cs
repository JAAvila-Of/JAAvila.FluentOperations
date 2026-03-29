using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is not entirely lower case.
/// </summary>
internal class StringNotBeLowerCasedValidator(PrincipalChain<string?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static StringNotBeLowerCasedValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected => "Not uppercase text.";
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.NotBeLowerCased";
    string IRuleDescriptor.OperationName => "NotBeLowerCased";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (!chain.GetValue().IsLowerCased())
        {
            return true;
        }

        ResultValidation = "The resulting value was not expected to be lowercased.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
