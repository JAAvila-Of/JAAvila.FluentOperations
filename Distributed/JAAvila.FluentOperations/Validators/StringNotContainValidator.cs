using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string does not contain the expected substring.
/// </summary>
internal class StringNotContainValidator(
    string expected,
    PrincipalChain<string?> chain,
    StringComparison comparison = StringComparison.Ordinal
) : IValidator, IRuleDescriptor
{
    public static StringNotContainValidator New(string expected, PrincipalChain<string?> chain) =>
        new(expected, chain);

    public static StringNotContainValidator New(
        string expected,
        PrincipalChain<string?> chain,
        StringComparison comparison
    ) => new(expected, chain, comparison);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.NotContain";
    string IRuleDescriptor.OperationName => "NotContain";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>
        {
            ["value"] = expected,
            ["comparison"] = nameof(StringComparison.Ordinal)
        };

    public bool Validate()
    {
        if (!chain.GetValue()!.Contains(expected, comparison))
        {
            return true;
        }

        ResultValidation = "The value was expected to not contain {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
