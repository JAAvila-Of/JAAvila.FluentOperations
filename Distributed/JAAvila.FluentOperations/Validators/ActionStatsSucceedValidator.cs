using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the action completed without throwing an exception.
/// </summary>
internal class ActionStatsSucceedValidator(PrincipalChain<Model.ActionStats?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ActionStatsSucceedValidator New(PrincipalChain<Model.ActionStats?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ActionStats.Succeed";
    string IRuleDescriptor.OperationName => "Succeed";
    Type IRuleDescriptor.SubjectType => typeof(Model.ActionStats);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    /// <summary>
    /// Returns the exception info string for use in the failure message template.
    /// </summary>
    public string ExceptionInfo
    {
        get
        {
            var ex = chain.GetValue()?.Exception;
            return ex is not null
                ? $" {ex.GetType().Name} was thrown with message: \"{ex.Message}\""
                : "";
        }
    }

    public bool Validate()
    {
        if (chain.GetValue()!.Succeeded)
        {
            return true;
        }

        ResultValidation = "Expected the action to succeed, but it failed.{0}";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
