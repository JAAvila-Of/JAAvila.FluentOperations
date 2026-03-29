using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the elapsed time is between min and max (inclusive).
/// </summary>
internal class ActionStatsHaveElapsedTimeBetweenValidator(
    PrincipalChain<Model.ActionStats?> chain,
    TimeSpan min,
    TimeSpan max
) : IValidator, IRuleDescriptor
{
    public static ActionStatsHaveElapsedTimeBetweenValidator New(
        PrincipalChain<Model.ActionStats?> chain,
        TimeSpan min,
        TimeSpan max
    ) => new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ActionStats.HaveElapsedTimeBetween";
    string IRuleDescriptor.OperationName => "HaveElapsedTimeBetween";
    Type IRuleDescriptor.SubjectType => typeof(Model.ActionStats);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value_min"] = min, ["value_max"] = max };

    public bool Validate()
    {
        var elapsed = chain.GetValue()!.ElapsedTime;

        if (elapsed >= min && elapsed <= max)
        {
            return true;
        }

        ResultValidation = "Expected the elapsed time to be between {0} and {1}, but it was {2}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
