using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

internal class AsyncPredicateRule<TProp>(
    Func<TProp, Task<bool>> predicate,
    string failureMessage,
    RuleConfig? config = null
) : IQualityRule, IAsyncQualityRule, IModelAwareRule
{
    private TProp? _value;
    private object? _currentModel;

    public bool Validate()
    {
        // Fallback for sync execution: run the async predicate synchronously
        // This is safe because CheckAsync() will use ValidateAsync() for IAsyncQualityRule
        return ValidateAsync().GetAwaiter().GetResult();
    }

    public async Task<bool> ValidateAsync()
    {
        return await predicate(_value!);
    }

    public string GetReport()
    {
        return GetCustomMessage() ?? failureMessage;
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

    public string? GetCustomMessage()
    {
        if (config?.MessageFactory is { } factory && _currentModel is not null)
        {
            return factory(_currentModel);
        }

        return config?.CustomMessage;
    }

    void IModelAwareRule.SetModelInstance(object model) => _currentModel = model;
}
