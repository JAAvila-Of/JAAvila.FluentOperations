using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string value is a valid GUID.
/// </summary>
internal class StringBeGuidValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeGuidValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.BeGuid";
    string IRuleDescriptor.OperationName => "BeGuid";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (Guid.TryParse(chain.GetValue()!, out _))
        {
            return true;
        }

        ResultValidation = "The value was expected to be a valid GUID.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
