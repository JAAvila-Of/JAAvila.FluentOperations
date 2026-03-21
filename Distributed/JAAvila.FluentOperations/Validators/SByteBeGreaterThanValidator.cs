using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the sbyte value is greater than the expected value.
/// </summary>
internal class SByteBeGreaterThanValidator(PrincipalChain<sbyte> chain, sbyte expected) : IValidator, IRuleDescriptor
{
    public static SByteBeGreaterThanValidator New(PrincipalChain<sbyte> chain, sbyte expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "SByte.BeGreaterThan";
    string IRuleDescriptor.OperationName => "BeGreaterThan";
    Type IRuleDescriptor.SubjectType => typeof(sbyte);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() > expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be greater than {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
