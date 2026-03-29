using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timeonly value has the expected hour.
/// </summary>
internal class TimeOnlyHaveHourValidator(PrincipalChain<TimeOnly> chain, int hour)
    : IValidator,
        IRuleDescriptor
{
    public static TimeOnlyHaveHourValidator New(PrincipalChain<TimeOnly> chain, int hour) =>
        new(chain, hour);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "TimeOnly.HaveHour";
    string IRuleDescriptor.OperationName => "HaveHour";
    Type IRuleDescriptor.SubjectType => typeof(TimeOnly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = hour };

    public bool Validate()
    {
        if (chain.GetValue().Hour == hour)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have hour {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
