using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

/// <summary>
/// A decorator that wraps an <see cref="IQualityRule"/> captured during Blueprint definition,
/// enriching it with the property name it belongs to, an optional scenario filter, and an
/// optional <see cref="RuleConfig"/> that can override the rule's severity, error code, and
/// custom message. All <see cref="IQualityRule"/> members delegate to <see cref="Inner"/>,
/// except the metadata accessors which prefer the <see cref="Config"/> values when present.
/// </summary>
internal class CapturedRule(string propertyName, IQualityRule inner, Type? scenario = null, RuleConfig? config = null)
    : IQualityRule
{
    /// <summary>Gets the name of the model property this rule is bound to.</summary>
    public string PropertyName { get; } = propertyName;

    /// <summary>Gets the underlying <see cref="IQualityRule"/> being decorated.</summary>
    public IQualityRule Inner { get; } = inner;

    /// <summary>Gets the optional scenario type that restricts when this rule is evaluated, or <c>null</c> for all scenarios.</summary>
    public Type? Scenario { get; } = scenario;

    /// <summary>Gets the optional per-rule configuration that overrides severity, error code, and custom message.</summary>
    public RuleConfig? Config { get; } = config;

    public bool Validate() => Inner.Validate();

    public Task<bool> ValidateAsync() => Inner.ValidateAsync();

    public string GetReport() => Inner.GetReport();

    public void SetValue(object? value) => Inner.SetValue(value);

    public Severity GetSeverity() => Config?.Severity ?? Inner.GetSeverity();
    public string? GetErrorCode() => Config?.ErrorCode ?? Inner.GetErrorCode();
    public string? GetCustomMessage() => Config?.CustomMessage ?? Inner.GetCustomMessage();
}
