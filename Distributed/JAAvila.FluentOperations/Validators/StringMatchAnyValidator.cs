using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string matches at least one of the specified regular expressions.
/// </summary>
internal class StringMatchAnyValidator(string[] patterns, PrincipalChain<string?> chain)
    : IValidator, IRuleDescriptor
{
    public static StringMatchAnyValidator New(string[] patterns, PrincipalChain<string?> chain) =>
        new(patterns, chain);

    public string Expected => "Match at least one of the provided patterns";
    public string ResultValidation { get; set; }
    public string MessageKey => "String.MatchAny";
    string IRuleDescriptor.OperationName => "MatchAny";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = patterns };

    public bool Validate()
    {
        if (patterns.Any(p => SafeIsMatch(chain.GetValue()!, p)))
        {
            return true;
        }

        ResultValidation =
            "The value was expected to match at least one of the provided patterns but matched none.";
        return false;
    }

    private static bool SafeIsMatch(string value, string pattern)
    {
        try
        {
            return Regex.IsMatch(value, pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
