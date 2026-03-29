using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is not empty.
/// </summary>
internal class StringNotBeEmptyValidator(PrincipalChain<string?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static StringNotBeEmptyValidator New(PrincipalChain<string?> chain) => new(chain);

    /// <inheritdoc />
    public string Expected => "Not be empty - \"\"";

    /// <inheritdoc />
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.NotBeEmpty";

    string IRuleDescriptor.OperationName => "NotBeEmpty";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    /// <inheritdoc />
    public bool Validate()
    {
        if (chain.GetValue()?.Length > 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be empty.";
        return false;
    }

    /// <inheritdoc />
    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
