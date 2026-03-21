using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string value is not null.
/// </summary>
internal class StringNotBeNullValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringNotBeNullValidator New(PrincipalChain<string?> chain) => new(chain);

    /// <inheritdoc />
    public string Expected => "Not Be Null - <not null>";

    /// <inheritdoc />
    public string ResultValidation { get; set; }
    public string MessageKey => "String.NotBeNull";

    string IRuleDescriptor.OperationName => "NotBeNull";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    /// <inheritdoc />
    public bool Validate()
    {
        if (chain.GetValue() is not null)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be null.";
        return false;
    }

    /// <inheritdoc />
    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
