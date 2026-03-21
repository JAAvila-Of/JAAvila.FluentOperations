using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value is close to the expected value within a specified precision.
/// </summary>
internal class NullableDateTimeOffsetBeCloseToValidator(
    PrincipalChain<DateTimeOffset?> chain,
    DateTimeOffset expected,
    TimeSpan tolerance
) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeOffsetBeCloseToValidator New(
        PrincipalChain<DateTimeOffset?> chain,
        DateTimeOffset expected,
        TimeSpan tolerance
    ) => new(chain, expected, tolerance);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTimeOffset.BeCloseTo";
    string IRuleDescriptor.OperationName => "BeCloseTo";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected, ["value"] = tolerance };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Math.Abs((value!.Value - expected).Ticks) <= tolerance.Ticks)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be close to {0} (tolerance: {1}), but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
