using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string starts with the expected substring.
/// </summary>
internal class StringStartWithValidator(
    string prefix,
    PrincipalChain<string?> chain,
    StringComparison comparison = StringComparison.Ordinal
) : IValidator, IRuleDescriptor
{
    public static StringStartWithValidator New(string prefix, PrincipalChain<string?> chain) =>
        new(prefix, chain);

    public static StringStartWithValidator New(
        string prefix,
        PrincipalChain<string?> chain,
        StringComparison comparison
    ) => new(prefix, chain, comparison);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.StartWith";
    string IRuleDescriptor.OperationName => "StartWith";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = prefix, ["comparison"] = StringComparison.Ordinal.ToString() };

    public bool Validate()
    {
        if (chain.GetValue()!.StartsWith(prefix, comparison))
        {
            return true;
        }

        ResultValidation = "The value was expected to start with \"{0}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
