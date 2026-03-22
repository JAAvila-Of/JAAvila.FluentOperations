using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable guid value is empty.
/// </summary>
internal class NullableGuidBeEmptyValidator(PrincipalChain<Guid?> chain) : IValidator, IRuleDescriptor
{
    public static NullableGuidBeEmptyValidator New(PrincipalChain<Guid?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableGuid.BeEmpty";
    string IRuleDescriptor.OperationName => "BeEmpty";
    Type IRuleDescriptor.SubjectType => typeof(Guid?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value == Guid.Empty)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be an empty GUID, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
