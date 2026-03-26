using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timespan value has the expected minutes component.
/// </summary>
internal class NullableTimeSpanHaveMinutesValidator(PrincipalChain<TimeSpan?> chain, int minutes) : IValidator, IRuleDescriptor
{
    public static NullableTimeSpanHaveMinutesValidator New(PrincipalChain<TimeSpan?> chain, int minutes) =>
        new(chain, minutes);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableTimeSpan.HaveMinutes";
    string IRuleDescriptor.OperationName => "HaveMinutes";
    Type IRuleDescriptor.SubjectType => typeof(TimeSpan?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = minutes };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Minutes == minutes)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have {0} minutes component, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
