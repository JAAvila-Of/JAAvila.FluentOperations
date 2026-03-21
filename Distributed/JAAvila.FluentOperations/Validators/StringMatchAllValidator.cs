using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string matches all of the specified regular expressions.
/// </summary>
internal class StringMatchAllValidator(string[] patterns, PrincipalChain<string?> chain)
    : IValidator, IRuleDescriptor
{
    public static StringMatchAllValidator New(string[] patterns, PrincipalChain<string?> chain) =>
        new(patterns, chain);

    public string Expected => "Match all of the provided patterns";
    public string ResultValidation { get; set; }
    public string MessageKey => "String.MatchAll";
    string IRuleDescriptor.OperationName => "MatchAll";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = patterns };

    public bool Validate()
    {
        if (patterns.All(p => SafeIsMatch(chain.GetValue()!, p)))
        {
            return true;
        }

        ResultValidation =
            "The value was expected to match all of the provided patterns but did not.";
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
