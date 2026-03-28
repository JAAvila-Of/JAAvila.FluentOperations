using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string contains all of the expected substrings.
/// </summary>
internal class StringContainAllValidator(
    PrincipalChain<string?> chain,
    string[] substrings,
    StringComparison? comparison
) : IValidator, IRuleDescriptor
{
    public static StringContainAllValidator New(
        PrincipalChain<string?> chain,
        string[] substrings,
        StringComparison? comparison = null
    ) => new(chain, substrings, comparison);

    public string Expected => string.Join(", ", substrings);
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.ContainAll";
    string IRuleDescriptor.OperationName => "ContainAll";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>
        {
            ["values"] = substrings,
            ["comparison"] = comparison?.ToString()!
        };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (substrings.All(s => value!.Contains(s, comparison ?? StringComparison.Ordinal)))
        {
            return true;
        }

        ResultValidation =
            "The value was expected to contain all of the specified substrings, but one or more were not found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
