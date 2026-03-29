using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the action took longer than the specified minimum duration.
/// </summary>
internal class ActionStatsTakeLongerThanValidator(
    PrincipalChain<Model.ActionStats?> chain,
    TimeSpan minDuration
) : IValidator, IRuleDescriptor
{
    public static ActionStatsTakeLongerThanValidator New(
        PrincipalChain<Model.ActionStats?> chain,
        TimeSpan minDuration
    ) => new(chain, minDuration);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ActionStats.TakeLongerThan";
    string IRuleDescriptor.OperationName => "TakeLongerThan";
    Type IRuleDescriptor.SubjectType => typeof(Model.ActionStats);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = minDuration };

    public bool Validate()
    {
        if (chain.GetValue()!.ElapsedTime >= minDuration)
        {
            return true;
        }

        ResultValidation = "Expected the action to take longer than {0}, but it completed in {1}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
