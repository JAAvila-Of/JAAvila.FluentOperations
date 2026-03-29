using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timeonly value is within the specified inclusive range.
/// </summary>
internal class NullableTimeOnlyBeInRangeValidator(
    PrincipalChain<TimeOnly?> chain,
    TimeOnly min,
    TimeOnly max
) : IValidator, IRuleDescriptor
{
    public static NullableTimeOnlyBeInRangeValidator New(
        PrincipalChain<TimeOnly?> chain,
        TimeOnly min,
        TimeOnly max
    ) => new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableTimeOnly.BeInRange";
    string IRuleDescriptor.OperationName => "BeInRange";
    Type IRuleDescriptor.SubjectType => typeof(TimeOnly?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min, ["max"] = max };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.IsBetween(min, max))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the given range, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
