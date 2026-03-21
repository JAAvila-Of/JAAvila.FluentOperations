using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the action completed within the specified maximum duration.
/// </summary>
internal class ActionStatsCompleteWithinValidator(PrincipalChain<Model.ActionStats?> chain, TimeSpan maxDuration) : IValidator, IRuleDescriptor
{
    public static ActionStatsCompleteWithinValidator New(PrincipalChain<Model.ActionStats?> chain, TimeSpan maxDuration) =>
        new(chain, maxDuration);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "ActionStats.CompleteWithin";
    string IRuleDescriptor.OperationName => "CompleteWithin";
    Type IRuleDescriptor.SubjectType => typeof(Model.ActionStats);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = maxDuration };

    public bool Validate()
    {
        if (chain.GetValue()!.ElapsedTime <= maxDuration)
        {
            return true;
        }

        ResultValidation = "Expected the action to complete within {0}, but it took {1}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
