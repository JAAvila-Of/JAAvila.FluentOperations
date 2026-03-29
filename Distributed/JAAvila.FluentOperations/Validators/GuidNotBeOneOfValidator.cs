using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the guid value is not one of the specified disallowed values.
/// </summary>
internal class GuidNotBeOneOfValidator(PrincipalChain<Guid> chain, params Guid[] expected)
    : IValidator,
        IRuleDescriptor
{
    public static GuidNotBeOneOfValidator New(PrincipalChain<Guid> chain, params Guid[] expected) =>
        new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Guid.NotBeOneOf";
    string IRuleDescriptor.OperationName => "NotBeOneOf";
    Type IRuleDescriptor.SubjectType => typeof(Guid);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = expected };

    public bool Validate()
    {
        if (!expected.Contains(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be one of [{0}].";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
