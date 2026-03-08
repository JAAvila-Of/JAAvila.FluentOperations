namespace JAAvila.FluentOperations;

/// <summary>
/// Caches the result of a condition function so it is evaluated at most once per Check() invocation.
/// All ConditionalRuleWrappers in the same When() group share a single ConditionGroup instance.
/// </summary>
internal sealed class ConditionGroup<TModel>
{
    private readonly Func<TModel, bool> _condition;
    private bool? _cachedResult;

    public ConditionGroup(Func<TModel, bool> condition)
    {
        _condition = condition;
    }

    /// <summary>
    /// Returns the cached condition result, evaluating it on first call per Check() cycle.
    /// </summary>
    public bool GetResult(TModel instance)
    {
        if (_cachedResult.HasValue)
        {
            return _cachedResult.Value;
        }

        _cachedResult = _condition(instance);
        return _cachedResult.Value;
    }

    /// <summary>
    /// Clears the cached result so the condition will be re-evaluated on the next Check() call.
    /// </summary>
    public void Reset()
    {
        _cachedResult = null;
    }
}
