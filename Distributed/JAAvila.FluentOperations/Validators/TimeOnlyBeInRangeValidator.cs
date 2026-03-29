using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timeonly value is within the specified inclusive range.
/// </summary>
internal class TimeOnlyBeInRangeValidator(
    PrincipalChain<TimeOnly> chain,
    TimeOnly min,
    TimeOnly max
) : IValidator, IRuleDescriptor
{
    public static TimeOnlyBeInRangeValidator New(
        PrincipalChain<TimeOnly> chain,
        TimeOnly min,
        TimeOnly max
    ) => new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "TimeOnly.BeInRange";
    string IRuleDescriptor.OperationName => "BeInRange";
    Type IRuleDescriptor.SubjectType => typeof(TimeOnly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min, ["max"] = max };

    public bool Validate()
    {
        if (chain.GetValue().IsBetween(min, max))
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
