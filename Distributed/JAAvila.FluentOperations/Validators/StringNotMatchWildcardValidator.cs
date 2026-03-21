using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Utils;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string does not match the expected wildcard pattern.
/// </summary>
internal class StringNotMatchWildcardValidator(
    PrincipalChain<string?> chain,
    string pattern,
    StringComparison comparison = StringComparison.Ordinal
) : IValidator, IRuleDescriptor
{
    public static StringNotMatchWildcardValidator New(
        PrincipalChain<string?> chain,
        string pattern,
        StringComparison comparison = StringComparison.Ordinal
    ) => new(chain, pattern, comparison);

    public string Expected => pattern;
    public string ResultValidation { get; set; }
    public string MessageKey => "String.NotMatchWildcard";
    string IRuleDescriptor.OperationName => "NotMatchWildcard";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["pattern"] = pattern, ["comparison"] = StringComparison.Ordinal.ToString() };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value is null || !WildcardMatcher.IsMatch(value, pattern, comparison))
        {
            return true;
        }

        ResultValidation =
            "The value was expected to not match wildcard pattern {0}, but {1} matched.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
