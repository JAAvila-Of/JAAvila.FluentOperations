using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timespan value has the expected seconds component.
/// </summary>
internal class NullableTimeSpanHaveSecondsValidator(PrincipalChain<TimeSpan?> chain, int seconds) : IValidator, IRuleDescriptor
{
    public static NullableTimeSpanHaveSecondsValidator New(PrincipalChain<TimeSpan?> chain, int seconds) =>
        new(chain, seconds);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableTimeSpan.HaveSeconds";
    string IRuleDescriptor.OperationName => "HaveSeconds";
    Type IRuleDescriptor.SubjectType => typeof(TimeSpan?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = seconds };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Seconds == seconds)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have {0} seconds component, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
