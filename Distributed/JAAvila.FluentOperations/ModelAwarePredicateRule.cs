using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

/// <summary>
/// A synchronous predicate rule that receives both the root model and the property value.
/// Enables cross-property validation predicates like:
/// <c>ForPredicate&lt;decimal&gt;(x =&gt; x.Discount, (order, discount) =&gt; discount &lt;= order.MaxDiscount, "...")</c>
/// </summary>
internal class ModelAwarePredicateRule<TModel, TProp>(
    Func<TModel, TProp, bool> predicate,
    string failureMessage,
    RuleConfig? config = null
) : IQualityRule, IModelAwareRule
{
    private TProp? _value;
    private TModel? _modelInstance;

    /// <inheritdoc />
    public void SetModelInstance(object model)
    {
        if (model is TModel typed)
        {
            _modelInstance = typed;
        }
    }

    public bool Validate()
    {
        if (_modelInstance is null)
        {
            // Model not injected — fail with a safe default.
            // This should not occur if the evaluation loops are correctly implemented.
            return false;
        }

        return predicate(_modelInstance, _value!);
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());

    public string GetReport()
    {
        return config?.CustomMessage ?? failureMessage;
    }

    public void SetValue(object? value)
    {
        _value = value switch
        {
            TProp typed => typed,
            null when !typeof(TProp).IsValueType => default!,
            _ => _value
        };
    }

    public Severity GetSeverity() => config?.Severity ?? Severity.Error;

    public string? GetErrorCode() => config?.ErrorCode;

    public string? GetCustomMessage() => config?.CustomMessage;
}
