using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string contains no whitespace characters.
/// </summary>
internal class StringContainNoWhitespaceValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringContainNoWhitespaceValidator New(PrincipalChain<string?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.ContainNoWhitespace";
    string IRuleDescriptor.OperationName => "ContainNoWhitespace";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (!chain.GetValue()!.Any(char.IsWhiteSpace))
        {
            return true;
        }

        ResultValidation = "The value was expected to contain no whitespace characters.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
