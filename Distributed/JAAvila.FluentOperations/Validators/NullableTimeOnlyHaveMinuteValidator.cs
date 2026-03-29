using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timeonly value has the expected minute.
/// </summary>
internal class NullableTimeOnlyHaveMinuteValidator(PrincipalChain<TimeOnly?> chain, int minute)
    : IValidator,
        IRuleDescriptor
{
    public static NullableTimeOnlyHaveMinuteValidator New(
        PrincipalChain<TimeOnly?> chain,
        int minute
    ) => new(chain, minute);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableTimeOnly.HaveMinute";
    string IRuleDescriptor.OperationName => "HaveMinute";
    Type IRuleDescriptor.SubjectType => typeof(TimeOnly?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = minute };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Minute == minute)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have minute {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
