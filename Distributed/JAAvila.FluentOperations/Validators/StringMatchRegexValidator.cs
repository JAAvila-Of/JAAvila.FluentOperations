using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string matches the specified precompiled regular expression.
/// </summary>
internal class StringMatchRegexValidator(Regex regex, PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringMatchRegexValidator New(Regex regex, PrincipalChain<string?> chain) =>
        new(regex, chain);

    public string Expected => $"Match pattern \"{regex}\"";
    public string ResultValidation { get; set; }
    public string MessageKey => "String.MatchRegex";

    string IRuleDescriptor.OperationName => "MatchRegex";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["pattern"] = regex.ToString() };

    public bool Validate()
    {
        try
        {
            if (regex.IsMatch(chain.GetValue()!))
            {
                return true;
            }

            ResultValidation = "The value was expected to match the pattern {0}.";
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
