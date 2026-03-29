using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

/// <summary>
/// An asynchronous predicate rule that receives both the root model and the property value.
/// Enables async cross-property validation like:
/// <c>ForPredicateAsync&lt;decimal&gt;(x =&gt; x.Discount,
///     async (order, discount) =&gt; discount &lt;= await repo.GetMax(order.ProductId), "...")</c>
/// </summary>
internal class ModelAwareAsyncPredicateRule<TModel, TProp>(
    Func<TModel, TProp, Task<bool>> predicate,
    string failureMessage,
    RuleConfig? config = null
) : IQualityRule, IModelAwareRule, IAsyncQualityRule
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
        // Fallback sync path — same pattern as AsyncPredicateRule
        return ValidateAsync().GetAwaiter().GetResult();
    }

    public async Task<bool> ValidateAsync()
    {
        if (_modelInstance is null)
        {
            // Model isn't injected — fail with a safe default.
            // This should not occur if the evaluation loops are correctly implemented.
            return false;
        }

        return await predicate(_modelInstance, _value!);
    }

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
