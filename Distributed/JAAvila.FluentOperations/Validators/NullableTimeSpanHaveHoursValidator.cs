using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timespan value has the expected hours component.
/// </summary>
internal class NullableTimeSpanHaveHoursValidator(PrincipalChain<TimeSpan?> chain, int hours) : IValidator, IRuleDescriptor
{
    public static NullableTimeSpanHaveHoursValidator New(PrincipalChain<TimeSpan?> chain, int hours) =>
        new(chain, hours);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableTimeSpan.HaveHours";
    string IRuleDescriptor.OperationName => "HaveHours";
    Type IRuleDescriptor.SubjectType => typeof(TimeSpan?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = hours };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Hours == hours)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have {0} hours component, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
