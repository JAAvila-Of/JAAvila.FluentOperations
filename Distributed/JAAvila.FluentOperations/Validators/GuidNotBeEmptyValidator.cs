using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the guid is not empty.
/// </summary>
internal class GuidNotBeEmptyValidator(PrincipalChain<Guid> chain) : IValidator, IRuleDescriptor
{
    public static GuidNotBeEmptyValidator New(PrincipalChain<Guid> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Guid.NotBeEmpty";
    string IRuleDescriptor.OperationName => "NotBeEmpty";
    Type IRuleDescriptor.SubjectType => typeof(Guid);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() != Guid.Empty)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be an empty GUID.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
