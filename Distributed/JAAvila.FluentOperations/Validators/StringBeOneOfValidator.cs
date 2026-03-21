using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string value is one of the specified allowed values.
/// </summary>
internal class StringBeOneOfValidator(
    PrincipalChain<string?> chain,
    string?[] expected,
    StringComparison? comparison
) : IValidator, IRuleDescriptor
{
    public static StringBeOneOfValidator New(
        PrincipalChain<string?> chain,
        string?[] expected,
        StringComparison? comparison = null
    ) => new(chain, expected, comparison);

    public string Expected { get; } = string.Join(", ", expected.Select(e => e is null ? "<null>" : $"\"{e}\""));
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeOneOf";

    string IRuleDescriptor.OperationName => "BeOneOf";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = expected };

    public bool Validate()
    {
        var value = chain.GetValue();
        var comp = comparison ?? StringComparison.Ordinal;

        if (expected.Any(e => string.Equals(value, e, comp)))
        {
            return true;
        }

        ResultValidation = comparison.HasValue
            ? $"The resulting value was expected to be one of [{{0}}] using {comp}, but {{1}} was found."
            : "The resulting value was expected to be one of [{0}], but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
