using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the sbyte value is greater than or equal to the expected value.
/// </summary>
internal class SByteBeGreaterThanOrEqualToValidator(PrincipalChain<sbyte> chain, sbyte expected)
    : IValidator,
        IRuleDescriptor
{
    public static SByteBeGreaterThanOrEqualToValidator New(
        PrincipalChain<sbyte> chain,
        sbyte expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "SByte.BeGreaterThanOrEqualTo";
    string IRuleDescriptor.OperationName => "BeGreaterThanOrEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(sbyte);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() >= expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be greater than or equal to {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
