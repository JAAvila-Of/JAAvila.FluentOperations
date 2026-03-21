using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string does not match the expected regular expression.
/// </summary>
internal class StringNotMatchValidator(string pattern, PrincipalChain<string?> chain) : IValidator
{
    public static StringNotMatchValidator New(string pattern, PrincipalChain<string?> chain) =>
        new(pattern, chain);

    public string Expected => $"Not match pattern \"{pattern}\"";
    public string ResultValidation { get; set; }
    public string MessageKey => "String.NotMatch";

    public bool Validate()
    {
        try
        {
            if (
                !Regex.IsMatch(
                    chain.GetValue()!,
                    pattern,
                    RegexOptions.None,
                    TimeSpan.FromSeconds(1)
                )
            )
            {
                return true;
            }

            ResultValidation = "The value was expected to not match the pattern {0}.";
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
