using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string ends with the expected substring.
/// </summary>
internal class StringEndWithValidator(
    string suffix,
    PrincipalChain<string?> chain,
    StringComparison comparison = StringComparison.Ordinal
) : IValidator, IRuleDescriptor
{
    public static StringEndWithValidator New(string suffix, PrincipalChain<string?> chain) =>
        new(suffix, chain);

    public static StringEndWithValidator New(
        string suffix,
        PrincipalChain<string?> chain,
        StringComparison comparison
    ) => new(suffix, chain, comparison);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.EndWith";
    string IRuleDescriptor.OperationName => "EndWith";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>
        {
            ["value"] = suffix,
            ["comparison"] = nameof(StringComparison.Ordinal)
        };

    public bool Validate()
    {
        if (chain.GetValue()!.EndsWith(suffix, comparison))
        {
            return true;
        }

        ResultValidation = "The value was expected to end with \"{0}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
