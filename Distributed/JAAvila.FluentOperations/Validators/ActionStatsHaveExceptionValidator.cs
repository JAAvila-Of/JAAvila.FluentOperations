using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the action threw an exception of the specified type (or derived).
/// </summary>
internal class ActionStatsHaveExceptionValidator(PrincipalChain<Model.ActionStats?> chain, Type expectedExceptionType) : IValidator, IRuleDescriptor
{
    public static ActionStatsHaveExceptionValidator New(PrincipalChain<Model.ActionStats?> chain, Type expectedExceptionType) =>
        new(chain, expectedExceptionType);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "ActionStats.HaveException";
    string IRuleDescriptor.OperationName => "HaveException";
    Type IRuleDescriptor.SubjectType => typeof(Model.ActionStats);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["type"] = expectedExceptionType.FullName ?? expectedExceptionType.Name };

    /// <summary>
    /// Indicates whether the failure was due to no exception being captured (true)
    /// or the wrong exception type being thrown (false).
    /// </summary>
    public bool NoExceptionCaptured { get; private set; }

    public bool Validate()
    {
        var stats = chain.GetValue()!;

        if (stats.Exception is null)
        {
            NoExceptionCaptured = true;
            ResultValidation = "Expected the action to have thrown {0}, but no exception was captured.";
            return false;
        }

        if (!expectedExceptionType.IsInstanceOfType(stats.Exception))
        {
            NoExceptionCaptured = false;
            ResultValidation = "Expected the action to have thrown {0}, but {1} was thrown instead.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
