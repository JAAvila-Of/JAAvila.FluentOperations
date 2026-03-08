using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

/// <summary>
/// Wraps a quality rule with a condition that determines whether it should execute.
/// When isOtherwise is true, the condition is negated.
/// Optionally shares a ConditionGroup to evaluate the condition once per group per Check().
/// </summary>
internal class ConditionalRuleWrapper<TModel>(
    IQualityRule inner,
    Func<TModel, bool> condition,
    bool isOtherwise = false,
    ConditionGroup<TModel>? conditionGroup = null
) : IQualityRule, IConditionalRule
{
    private TModel? _modelInstance;

    /// <summary>
    /// Sets the model instance for evaluating the condition.
    /// Must be called before Validate()/ValidateAsync().
    /// </summary>
    internal void SetModelInstance(TModel instance)
    {
        _modelInstance = instance;
    }

    public bool ShouldExecute()
    {
        if (_modelInstance is null)
        {
            return false;
        }

        var result = conditionGroup?.GetResult(_modelInstance) ?? condition(_modelInstance);

        return isOtherwise ? !result : result;
    }

    public bool Validate()
    {
        return !ShouldExecute() || inner.Validate();
    }

    public async Task<bool> ValidateAsync()
    {
        if (!ShouldExecute())
        {
            return true;
        }

        return await inner.ValidateAsync();
    }

    public string GetReport() => inner.GetReport();

    public void SetValue(object? value)
    {
        // Pass value to the inner rule for actual validation
        inner.SetValue(value);
    }

    public Severity GetSeverity() => inner.GetSeverity();

    public string? GetErrorCode() => inner.GetErrorCode();

    public string? GetCustomMessage() => inner.GetCustomMessage();
}
