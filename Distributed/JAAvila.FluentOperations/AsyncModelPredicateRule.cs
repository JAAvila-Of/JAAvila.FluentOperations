using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

internal class AsyncModelPredicateRule<T>(
    Func<T, Task<bool>> predicate,
    string failureMessage,
    RuleConfig? config = null
) : IQualityRule, ICrossPropertyRule, IAsyncQualityRule
{
    private T? _instance;

    public bool Validate()
    {
        // Fallback for sync execution: run the async predicate synchronously
        // This is safe because CheckAsync() will use ValidateAsync() for IAsyncQualityRule
        return ValidateAsync().GetAwaiter().GetResult();
    }

    public async Task<bool> ValidateAsync()
    {
        if (_instance is null) return false;
        return await predicate(_instance);
    }

    public string GetReport()
    {
        return config?.CustomMessage ?? failureMessage;
    }

    public void SetValue(object? value)
    {
        if (value is T model)
        {
            _instance = model;
        }
    }

    public Severity GetSeverity() => config?.Severity ?? Severity.Error;

    public string? GetErrorCode() => config?.ErrorCode;

    public string? GetCustomMessage() => config?.CustomMessage;
}
