using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

internal enum CrossPropertyComparison
{
    Equal,
    NotEqual,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual,
}

internal class CrossPropertyRule<TModel, TProp>(
    string propertyName,
    string referencePropertyName,
    Func<TModel, TProp> valueExtractor,
    Func<TModel, TProp> referenceExtractor,
    CrossPropertyComparison comparison,
    RuleConfig? config = null,
    Reason? reason = null
) : IQualityRule, ICrossPropertyRule, IModelAwareRule
{
    private TModel? _instance;

    public bool Validate()
    {
        if (_instance is null)
        {
            return false;
        }

        var value = valueExtractor(_instance);
        var reference = referenceExtractor(_instance);

        return comparison switch
        {
            CrossPropertyComparison.Equal
                => EqualityComparer<TProp>.Default.Equals(value, reference),
            CrossPropertyComparison.NotEqual
                => !EqualityComparer<TProp>.Default.Equals(value, reference),
            CrossPropertyComparison.GreaterThan
                => value is IComparable<TProp> cv && cv.CompareTo(reference) > 0,
            CrossPropertyComparison.LessThan
                => value is IComparable<TProp> clv && clv.CompareTo(reference) < 0,
            CrossPropertyComparison.GreaterThanOrEqual
                => value is IComparable<TProp> cgev && cgev.CompareTo(reference) >= 0,
            CrossPropertyComparison.LessThanOrEqual
                => value is IComparable<TProp> clev && clev.CompareTo(reference) <= 0,
            _ => false,
        };
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());

    public string GetReport()
    {
        if (config?.CustomMessage is not null)
        {
            return config.CustomMessage;
        }

        var comparisonText = comparison switch
        {
            CrossPropertyComparison.Equal => "be equal to",
            CrossPropertyComparison.NotEqual => "be different from",
            CrossPropertyComparison.GreaterThan => "be greater than",
            CrossPropertyComparison.LessThan => "be less than",
            CrossPropertyComparison.GreaterThanOrEqual => "be greater than or equal to",
            CrossPropertyComparison.LessThanOrEqual => "be less than or equal to",
            _ => "compare to",
        };

        var value = _instance is not null ? valueExtractor(_instance) : default;
        var reference = _instance is not null ? referenceExtractor(_instance) : default;

        return $"Subject: {propertyName}\n"
            + $"Result: The value of {propertyName} ({value}) was expected to {comparisonText} {referencePropertyName} ({reference}).";
    }

    public void SetValue(object? value)
    {
        // For cross-property rules, SetValue receives the full model instance
        if (value is TModel model)
        {
            _instance = model;
        }
    }

    public Severity GetSeverity() => config?.Severity ?? Severity.Error;

    public string? GetErrorCode() => config?.ErrorCode;

    public string? GetCustomMessage()
    {
        if (config?.MessageFactory is { } factory && _instance is not null)
        {
            return factory(_instance);
        }

        return config?.CustomMessage;
    }

    void IModelAwareRule.SetModelInstance(object model)
    {
        // CrossPropertyRule already receives the model via SetValue(instance).
        // SetModelInstance is called before SetValue in the loop, so we store it here
        // to make MessageFactory available via GetCustomMessage().
        if (model is TModel typed)
        {
            _instance = typed;
        }
    }
}
