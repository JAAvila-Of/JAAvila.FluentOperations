using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the action completed in less than the specified duration.
/// </summary>
internal class ActionStatsTakeShorterThanValidator(PrincipalChain<Model.ActionStats?> chain, TimeSpan maxDuration) : IValidator, IRuleDescriptor
{
    public static ActionStatsTakeShorterThanValidator New(PrincipalChain<Model.ActionStats?> chain, TimeSpan maxDuration) =>
        new(chain, maxDuration);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "ActionStats.TakeShorterThan";
    string IRuleDescriptor.OperationName => "TakeShorterThan";
    Type IRuleDescriptor.SubjectType => typeof(Model.ActionStats);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = maxDuration };

    public bool Validate()
    {
        if (chain.GetValue()!.ElapsedTime < maxDuration)
        {
            return true;
        }

        ResultValidation = "Expected the action to take shorter than {0}, but it took {1}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
