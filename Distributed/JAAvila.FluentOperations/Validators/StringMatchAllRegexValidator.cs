using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string matches all of the specified precompiled regular expressions.
/// </summary>
internal class StringMatchAllRegexValidator(Regex[] patterns, PrincipalChain<string?> chain)
    : IValidator
{
    public static StringMatchAllRegexValidator New(Regex[] patterns, PrincipalChain<string?> chain) =>
        new(patterns, chain);

    public string Expected => "Match all of the provided patterns";
    public string ResultValidation { get; set; }
    public string MessageKey => "String.MatchAllRegex";

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

    private static bool SafeIsMatch(string value, Regex regex)
    {
        try
        {
            return regex.IsMatch(value);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
