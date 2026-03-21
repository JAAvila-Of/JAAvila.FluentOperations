using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string matches at least one of the specified precompiled regular expressions.
/// </summary>
internal class StringMatchAnyRegexValidator(Regex[] patterns, PrincipalChain<string?> chain)
    : IValidator
{
    public static StringMatchAnyRegexValidator New(Regex[] patterns, PrincipalChain<string?> chain) =>
        new(patterns, chain);

    public string Expected => "Match at least one of the provided patterns";
    public string ResultValidation { get; set; }
    public string MessageKey => "String.MatchAnyRegex";

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
