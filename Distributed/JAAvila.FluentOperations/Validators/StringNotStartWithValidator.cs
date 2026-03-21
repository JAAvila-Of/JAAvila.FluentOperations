using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string does not start with the expected substring.
/// </summary>
internal class StringNotStartWithValidator(string prefix, PrincipalChain<string?> chain)
    : IValidator, IRuleDescriptor
{
    public static StringNotStartWithValidator New(string prefix, PrincipalChain<string?> chain) =>
        new(prefix, chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.NotStartWith";
    string IRuleDescriptor.OperationName => "NotStartWith";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = prefix };

    public bool Validate()
    {
        if (!chain.GetValue()!.StartsWith(prefix, StringComparison.Ordinal))
        {
            return true;
        }

        ResultValidation = "The value was expected to not start with \"{0}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
