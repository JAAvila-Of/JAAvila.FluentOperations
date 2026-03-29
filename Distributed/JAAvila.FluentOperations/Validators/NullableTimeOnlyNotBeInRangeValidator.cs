using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timeonly value is outside the specified inclusive range.
/// </summary>
internal class NullableTimeOnlyNotBeInRangeValidator(
    PrincipalChain<TimeOnly?> chain,
    TimeOnly min,
    TimeOnly max
) : IValidator, IRuleDescriptor
{
    public static NullableTimeOnlyNotBeInRangeValidator New(
        PrincipalChain<TimeOnly?> chain,
        TimeOnly min,
        TimeOnly max
    ) => new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableTimeOnly.NotBeInRange";
    string IRuleDescriptor.OperationName => "NotBeInRange";
    Type IRuleDescriptor.SubjectType => typeof(TimeOnly?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min, ["max"] = max };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!value!.Value.IsBetween(min, max))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected not to be in the given range, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
