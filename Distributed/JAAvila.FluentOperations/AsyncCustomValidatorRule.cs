using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

internal class AsyncCustomValidatorRule<TProp>(
    IAsyncCustomValidator<TProp> validator,
    RuleConfig? config = null
) : IQualityRule, IAsyncQualityRule, IModelAwareRule
{
    private TProp? _value;
    private object? _currentModel;

    public bool Validate()
    {
        return ValidateAsync().GetAwaiter().GetResult();
    }

    public async Task<bool> ValidateAsync()
    {
        return await validator.IsValidAsync(_value!);
    }

    public string GetReport()
    {
        return GetCustomMessage() ?? validator.GetFailureMessage(_value!);
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
