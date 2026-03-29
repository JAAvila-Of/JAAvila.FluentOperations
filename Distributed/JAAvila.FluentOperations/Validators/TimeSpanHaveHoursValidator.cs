using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timespan value has the expected hours component.
/// </summary>
internal class TimeSpanHaveHoursValidator(PrincipalChain<TimeSpan> chain, int hours)
    : IValidator,
        IRuleDescriptor
{
    public static TimeSpanHaveHoursValidator New(PrincipalChain<TimeSpan> chain, int hours) =>
        new(chain, hours);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "TimeSpan.HaveHours";
    string IRuleDescriptor.OperationName => "HaveHours";
    Type IRuleDescriptor.SubjectType => typeof(TimeSpan);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = hours };

    public bool Validate()
    {
        if (chain.GetValue().Hours == hours)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have {0} hours component, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
