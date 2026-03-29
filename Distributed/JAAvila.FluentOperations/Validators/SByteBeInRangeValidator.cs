using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the sbyte value is within the specified inclusive range.
/// </summary>
internal class SByteBeInRangeValidator(PrincipalChain<sbyte> chain, sbyte min, sbyte max)
    : IValidator,
        IRuleDescriptor
{
    public static SByteBeInRangeValidator New(PrincipalChain<sbyte> chain, sbyte min, sbyte max) =>
        new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "SByte.BeInRange";
    string IRuleDescriptor.OperationName => "BeInRange";
    Type IRuleDescriptor.SubjectType => typeof(sbyte);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min, ["max"] = max };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value >= min && value <= max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
