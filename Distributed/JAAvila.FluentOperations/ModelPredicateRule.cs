using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

internal class ModelPredicateRule<T>(
    Func<T, bool> predicate,
    string failureMessage,
    RuleConfig? config = null
) : IQualityRule, ICrossPropertyRule
{
    private T? _instance;

    public bool Validate()
    {
        if (_instance is null) return false;
        return predicate(_instance);
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());

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
