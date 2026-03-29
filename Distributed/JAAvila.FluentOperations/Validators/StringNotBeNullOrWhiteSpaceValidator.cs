using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is not null and contains non-whitespace characters.
/// </summary>
internal class StringNotBeNullOrWhiteSpaceValidator(PrincipalChain<string?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static StringNotBeNullOrWhiteSpaceValidator New(PrincipalChain<string?> chain) =>
        new(chain);

    /// <inheritdoc />
    public string Expected => "Not be white space - \" \"";

    /// <inheritdoc />
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.NotBeNullOrWhiteSpace";
    string IRuleDescriptor.OperationName => "NotBeNullOrWhiteSpace";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    /// <inheritdoc />
    public bool Validate()
    {
        if (chain.GetValue().HasContent())
        {
            return true;
        }

        ResultValidation = "The subject was not expected to be null or white space.";
        return false;
    }

    /// <inheritdoc />
    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
