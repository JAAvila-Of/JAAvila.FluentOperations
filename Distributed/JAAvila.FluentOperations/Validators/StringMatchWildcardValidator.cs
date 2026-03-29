using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Utils;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string matches the expected wildcard pattern.
/// </summary>
internal class StringMatchWildcardValidator(
    PrincipalChain<string?> chain,
    string pattern,
    StringComparison comparison = StringComparison.Ordinal
) : IValidator, IRuleDescriptor
{
    public static StringMatchWildcardValidator New(
        PrincipalChain<string?> chain,
        string pattern,
        StringComparison comparison = StringComparison.Ordinal
    ) => new(chain, pattern, comparison);

    public string Expected => pattern;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.MatchWildcard";
    string IRuleDescriptor.OperationName => "MatchWildcard";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>
        {
            ["pattern"] = pattern,
            ["comparison"] = nameof(StringComparison.Ordinal)
        };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (WildcardMatcher.IsMatch(value!, pattern, comparison))
        {
            return true;
        }

        ResultValidation =
            "The value was expected to match wildcard pattern {0}, but {1} did not match.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
