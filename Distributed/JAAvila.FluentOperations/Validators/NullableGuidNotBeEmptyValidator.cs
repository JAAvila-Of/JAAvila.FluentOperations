using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable guid value is not empty.
/// </summary>
internal class NullableGuidNotBeEmptyValidator(PrincipalChain<Guid?> chain) : IValidator, IRuleDescriptor
{
    public static NullableGuidNotBeEmptyValidator New(PrincipalChain<Guid?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableGuid.NotBeEmpty";
    string IRuleDescriptor.OperationName => "NotBeEmpty";
    Type IRuleDescriptor.SubjectType => typeof(Guid?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value != Guid.Empty)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be an empty GUID.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
