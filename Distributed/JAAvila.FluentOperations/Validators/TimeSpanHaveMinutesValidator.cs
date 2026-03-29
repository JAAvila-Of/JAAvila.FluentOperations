using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timespan value has the expected minutes component.
/// </summary>
internal class TimeSpanHaveMinutesValidator(PrincipalChain<TimeSpan> chain, int minutes)
    : IValidator,
        IRuleDescriptor
{
    public static TimeSpanHaveMinutesValidator New(PrincipalChain<TimeSpan> chain, int minutes) =>
        new(chain, minutes);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "TimeSpan.HaveMinutes";
    string IRuleDescriptor.OperationName => "HaveMinutes";
    Type IRuleDescriptor.SubjectType => typeof(TimeSpan);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = minutes };

    public bool Validate()
    {
        if (chain.GetValue().Minutes == minutes)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have {0} minutes component, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
