using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the guid is empty.
/// </summary>
internal class GuidBeEmptyValidator(PrincipalChain<Guid> chain) : IValidator, IRuleDescriptor
{
    public static GuidBeEmptyValidator New(PrincipalChain<Guid> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Guid.BeEmpty";
    string IRuleDescriptor.OperationName => "BeEmpty";
    Type IRuleDescriptor.SubjectType => typeof(Guid);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() == Guid.Empty)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be an empty GUID, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
