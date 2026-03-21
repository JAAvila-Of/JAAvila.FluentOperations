using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the guid value does not equal the expected value.
/// </summary>
internal class GuidNotBeValidator(PrincipalChain<Guid> chain, Guid expected) : IValidator, IRuleDescriptor
{
    public static GuidNotBeValidator New(PrincipalChain<Guid> chain, Guid expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Guid.NotBe";
    string IRuleDescriptor.OperationName => "NotBe";
    Type IRuleDescriptor.SubjectType => typeof(Guid);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
