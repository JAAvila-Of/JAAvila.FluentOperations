using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timespan value has the expected days component.
/// </summary>
internal class NullableTimeSpanHaveDaysValidator(PrincipalChain<TimeSpan?> chain, int days)
    : IValidator,
        IRuleDescriptor
{
    public static NullableTimeSpanHaveDaysValidator New(
        PrincipalChain<TimeSpan?> chain,
        int days
    ) => new(chain, days);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableTimeSpan.HaveDays";
    string IRuleDescriptor.OperationName => "HaveDays";
    Type IRuleDescriptor.SubjectType => typeof(TimeSpan?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = days };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Days == days)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have {0} days component, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
