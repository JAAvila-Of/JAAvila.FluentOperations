using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is not entirely upper case.
/// </summary>
internal class StringNotBeUpperCasedValidator(PrincipalChain<string?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static StringNotBeUpperCasedValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected => "Not uppercase text.";
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.NotBeUpperCased";
    string IRuleDescriptor.OperationName => "NotBeUpperCased";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (!chain.GetValue().IsUpperCased())
        {
            return true;
        }

        ResultValidation = "The resulting value was not expected to be uppercased.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
