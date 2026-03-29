using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value is within the specified inclusive range.
/// </summary>
internal class DateTimeOffsetBeInRangeValidator(
    PrincipalChain<DateTimeOffset> chain,
    DateTimeOffset min,
    DateTimeOffset max
) : IValidator, IRuleDescriptor
{
    public static DateTimeOffsetBeInRangeValidator New(
        PrincipalChain<DateTimeOffset> chain,
        DateTimeOffset min,
        DateTimeOffset max
    ) => new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "DateTimeOffset.BeInRange";
    string IRuleDescriptor.OperationName => "BeInRange";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value_min"] = min, ["value_max"] = max };

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
