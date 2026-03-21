using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is empty.
/// </summary>
internal class StringBeEmptyValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeEmptyValidator New(PrincipalChain<string?> chain) => new(chain);

    /// <inheritdoc />
    public string Expected => "Be empty - \"\"";

    /// <inheritdoc />
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeEmpty";
    string IRuleDescriptor.OperationName => "BeEmpty";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    /// <inheritdoc />
    public bool Validate()
    {
        if (chain.GetValue()?.Length == 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be empty, but {0} was found.";
        return false;
    }

    /// <inheritdoc />
    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
