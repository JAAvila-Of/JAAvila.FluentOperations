using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

internal class CustomValidatorRule<TProp>(
    ICustomValidator<TProp> validator,
    RuleConfig? config = null
) : IQualityRule
{
    private TProp? _value;

    public bool Validate()
    {
        return validator.IsValid(_value!);
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());

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
