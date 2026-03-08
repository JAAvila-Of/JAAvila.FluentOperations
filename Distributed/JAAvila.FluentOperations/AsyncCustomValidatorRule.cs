using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

internal class AsyncCustomValidatorRule<TProp>(
    IAsyncCustomValidator<TProp> validator,
    RuleConfig? config = null
) : IQualityRule, IAsyncQualityRule
{
    private TProp? _value;

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
        return config?.CustomMessage ?? validator.GetFailureMessage(_value!);
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
