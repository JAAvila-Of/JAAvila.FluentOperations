using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timespan value is outside the specified inclusive range.
/// </summary>
internal class NullableTimeSpanNotBeInRangeValidator(
    PrincipalChain<TimeSpan?> chain,
    TimeSpan min,
    TimeSpan max
) : IValidator, IRuleDescriptor
{
    public static NullableTimeSpanNotBeInRangeValidator New(
        PrincipalChain<TimeSpan?> chain,
        TimeSpan min,
        TimeSpan max
    ) => new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableTimeSpan.NotBeInRange";
    string IRuleDescriptor.OperationName => "NotBeInRange";
    Type IRuleDescriptor.SubjectType => typeof(TimeSpan?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value_min"] = min, ["value_max"] = max };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < min || value > max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected not to be in the given range, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
