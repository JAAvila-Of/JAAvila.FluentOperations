using System.Text.RegularExpressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is a valid semantic version (SemVer).
/// </summary>
internal class StringBeSemVerValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeSemVerValidator New(PrincipalChain<string?> chain) => new(chain);

    // SemVer 2.0.0 official regex from semver.org
    private static readonly Regex SemVerRegex =
        new(
            @"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$",
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(1)
        );

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.BeSemVer";
    string IRuleDescriptor.OperationName => "BeSemVer";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!IsValidSemVer(value))
        {
            ResultValidation =
                "The value was expected to be a valid semantic version (SemVer 2.0.0 format).";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());

    private static bool IsValidSemVer(string? value)
    {
        if (value is null)
        {
            return false;
        }

        try
        {
            return SemVerRegex.IsMatch(value);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
