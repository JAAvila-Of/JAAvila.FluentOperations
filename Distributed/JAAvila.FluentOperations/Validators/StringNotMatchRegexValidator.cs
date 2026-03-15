using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string does not match the specified precompiled regular expression.
/// </summary>
internal class StringNotMatchRegexValidator(Regex regex, PrincipalChain<string?> chain) : IValidator
{
    public static StringNotMatchRegexValidator New(Regex regex, PrincipalChain<string?> chain) =>
        new(regex, chain);

    public string Expected => $"Not match pattern \"{regex}\"";
    public string ResultValidation { get; set; } = string.Empty;

    public bool Validate()
    {
        try
        {
            if (!regex.IsMatch(chain.GetValue()!))
            {
                return true;
            }

            ResultValidation = "The value was expected to not match the pattern {0}.";
            return false;
        }
        catch (RegexMatchTimeoutException)
        {
            ResultValidation =
                "The regex pattern timed out. Consider increasing the Regex timeout or simplifying the pattern.";
            return false;
        }
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
