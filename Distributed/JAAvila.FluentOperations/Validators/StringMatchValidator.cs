using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string matches the expected regular expression.
/// </summary>
internal class StringMatchValidator(string pattern, PrincipalChain<string?> chain) : IValidator
{
    public static StringMatchValidator New(string pattern, PrincipalChain<string?> chain) =>
        new(pattern, chain);

    public string Expected => $"Match pattern \"{pattern}\"";
    public string ResultValidation { get; set; }
    public string MessageKey => "String.Match";

    public bool Validate()
    {
        try
        {
            if (
                Regex.IsMatch(
                    chain.GetValue()!,
                    pattern,
                    RegexOptions.None,
                    TimeSpan.FromSeconds(1)
                )
            )
            {
                return true;
            }

            ResultValidation = "The value was expected to match the pattern {0}.";
            return false;
        }
        catch (RegexMatchTimeoutException)
        {
            ResultValidation =
                "The regex pattern timed out after 1 second. The pattern may be vulnerable to ReDoS.";
            return false;
        }
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
