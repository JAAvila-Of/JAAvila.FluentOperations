using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value is outside the specified inclusive range.
/// </summary>
internal class DateTimeNotBeInRangeValidator(
    PrincipalChain<DateTime> chain,
    DateTime min,
    DateTime max
) : IValidator, IRuleDescriptor
{
    public static DateTimeNotBeInRangeValidator New(
        PrincipalChain<DateTime> chain,
        DateTime min,
        DateTime max
    ) => new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "DateTime.NotBeInRange";
    string IRuleDescriptor.OperationName => "NotBeInRange";
    Type IRuleDescriptor.SubjectType => typeof(DateTime);
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
            "The resulting value was expected not to be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
