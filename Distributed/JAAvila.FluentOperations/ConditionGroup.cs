using System.Collections.Concurrent;

// ReSharper disable InconsistentNaming

namespace JAAvila.FluentOperations;

/// <summary>
/// Holds the per-Check() condition result cache and model instance as AsyncLocal values so that
/// each concurrent Check() invocation gets its own isolated slot. Parallel tasks spawned within
/// the same Check() call inherit the parent's values, ensuring they share the same cache and model
/// (correct behavior: same Check() call, same condition results, and model instance).
/// </summary>
internal static class ConditionCacheContext
{
    private static readonly AsyncLocal<ConcurrentDictionary<object, bool>?> _current = new();
    private static readonly AsyncLocal<object?> _modelInstance = new();

    public static ConcurrentDictionary<object, bool>? Current
    {
        get => _current.Value;
        set => _current.Value = value;
    }

    /// <summary>
    /// The root model instance for the current Check() call.
    /// Used by <see cref="ConditionGroup{TModel}.GetResult"/> to evaluate the condition
    /// without relying on mutable instance fields on shared singleton rule wrappers.
    /// </summary>
    public static object? ModelInstance
    {
        get => _modelInstance.Value;
        set => _modelInstance.Value = value;
    }
}

/// <summary>
/// Caches the result of a condition function so it is evaluated at most once per Check() invocation.
/// All ConditionalRuleWrappers in the same When() group share a single ConditionGroup instance.
/// </summary>
internal sealed class ConditionGroup<TModel>
{
    private readonly Func<TModel, bool> _condition;

    public ConditionGroup(Func<TModel, bool> condition)
    {
        _condition = condition;
    }

    /// <summary>
    /// Returns the cached condition result, evaluating and storing it on the first call per Check() cycle.
    /// The cache is read from <see cref="ConditionCacheContext.Current"/>, which is set at the start
    /// of each Check() / CheckAsync() call and is isolated per concurrent invocation via AsyncLocal.
    /// </summary>
    /// <param name="fallbackInstance">
    /// The model instance to use when <see cref="ConditionCacheContext.ModelInstance"/> is not set.
    /// In the normal singleton-blueprint and concurrent-Check() flow, the context carries the model.
    /// </param>
    public bool GetResult(TModel fallbackInstance)
    {
        // Prefer the context-carried model instance to avoid races on the caller's field.
        var modelInstance = ConditionCacheContext.ModelInstance is TModel contextModel
            ? contextModel
            : fallbackInstance;

        var cache = ConditionCacheContext.Current;

        return cache?.GetOrAdd(this, _ => _condition(modelInstance)) ?? _condition(modelInstance);
    }
}
