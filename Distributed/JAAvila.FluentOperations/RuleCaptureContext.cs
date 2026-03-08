using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

/// <summary>
/// Async-local context used during Blueprint rule definition to capture <see cref="IQualityRule"/>
/// instances as they are created by manager method calls. It tracks the active property name,
/// scenario filter, and optional <see cref="RuleConfig"/> so that each captured rule is stored
/// with the correct metadata. This context is active only inside a collecting scope initiated
/// by <see cref="StartCollecting"/> or <see cref="BeginPropertyCapture"/>.
/// </summary>
internal static class RuleCaptureContext
{
    private static readonly AsyncLocal<List<IQualityRule>?> RulesCollection = new();
    private static readonly AsyncLocal<string?> CurrentPropertyNameContext = new();
    private static readonly AsyncLocal<Type?> CurrentScenarioContext = new();
    private static readonly AsyncLocal<RuleConfig?> CurrentRuleConfigContext = new();

    /// <summary>Gets the property name currently being captured, or <c>null</c> outside a capture scope.</summary>
    public static string? CurrentPropertyName => CurrentPropertyNameContext.Value;

    /// <summary>Gets the scenario type filter active in the current capture scope, or <c>null</c> if none.</summary>
    public static Type? CurrentScenario => CurrentScenarioContext.Value;

    /// <summary>Gets the <see cref="RuleConfig"/> to attach to the next captured rule, or <c>null</c> if none.</summary>
    public static RuleConfig? CurrentRuleConfig => CurrentRuleConfigContext.Value;

    /// <summary>Gets a value indicating whether a rule-collection scope is currently active.</summary>
    public static bool IsCollecting => RulesCollection.Value is not null;

    /// <summary>
    /// Opens a rule-collection scope that routes all subsequent <see cref="AddRule"/> calls into
    /// <paramref name="rules"/>. Returns a disposable that ends the scope when disposed.
    /// </summary>
    /// <param name="rules">The list that will receive captured rules.</param>
    /// <param name="propertyName">Optional initial property name for the scope.</param>
    /// <param name="scenario">Optional scenario type to associate with captured rules.</param>
    /// <returns>An <see cref="IDisposable"/> that clears the collection context on disposal.</returns>
    public static IDisposable StartCollecting(
        List<IQualityRule> rules,
        string? propertyName = null,
        Type? scenario = null
    )
    {
        RulesCollection.Value = rules;
        CurrentPropertyNameContext.Value = propertyName;
        CurrentScenarioContext.Value = scenario;
        CurrentRuleConfigContext.Value = null;
        return new Scope();
    }

    /// <summary>
    /// Switches the active collection target and property name without clearing the current
    /// <see cref="RuleConfig"/>. Called by <c>PropertyProxy.Test()</c> before returning each
    /// manager so that rules land in the correct group with the correct metadata.
    /// </summary>
    /// <param name="rules">The list that will receive captured rules for this property.</param>
    /// <param name="propertyName">The property name to associate with captured rules.</param>
    /// <param name="scenario">Optional scenario type filter for captured rules.</param>
    public static void BeginPropertyCapture(
        List<IQualityRule> rules,
        string propertyName,
        Type? scenario = null
    )
    {
        RulesCollection.Value = rules;
        CurrentPropertyNameContext.Value = propertyName;
        CurrentScenarioContext.Value = scenario;
        // NOTE: CurrentRuleConfig is intentionally NOT cleared here.
        // It is set by SetRuleConfig() in the For(expr, config) overload,
        // and cleared by EndPropertyCapture() or by the For(expr) overload (no config) via SetRuleConfig(null).
    }

    /// <summary>
    /// Sets the <see cref="RuleConfig"/> that will be attached to the next captured rule.
    /// Pass <c>null</c> to clear any previously set config (used by the no-config <c>For()</c> overload).
    /// </summary>
    /// <param name="config">The config to attach, or <c>null</c> to clear.</param>
    public static void SetRuleConfig(RuleConfig? config)
    {
        CurrentRuleConfigContext.Value = config;
    }

    /// <summary>
    /// Overrides the scenario type for the current capture scope.
    /// </summary>
    /// <param name="scenario">The scenario type to set, or <c>null</c> to clear.</param>
    public static void SetScenario(Type? scenario)
    {
        CurrentScenarioContext.Value = scenario;
    }

    /// <summary>
    /// Ends the current property capture scope and resets all context values to <c>null</c>.
    /// </summary>
    public static void EndPropertyCapture()
    {
        RulesCollection.Value = null;
        CurrentPropertyNameContext.Value = null;
        CurrentScenarioContext.Value = null;
        CurrentRuleConfigContext.Value = null;
    }

    /// <summary>
    /// Wraps <paramref name="rule"/> in a <see cref="CapturedRule"/> with the current property name,
    /// scenario, and config, then adds it to the active rules collection.
    /// No-ops when no collection scope is active.
    /// </summary>
    /// <param name="rule">The rule to capture.</param>
    /// <param name="scenarioOverride">Optional scenario that overrides the context value.</param>
    public static void AddRule(IQualityRule rule, Type? scenarioOverride = null)
    {
        if (RulesCollection.Value is null)
        {
            return;
        }

        var property = CurrentPropertyName ?? string.Empty;
        var scenario = scenarioOverride ?? CurrentScenario;
        var config = CurrentRuleConfig;

        RulesCollection.Value.Add(new CapturedRule(property, rule, scenario, config));
    }

    private class Scope : IDisposable
    {
        public void Dispose()
        {
            RulesCollection.Value = null;
            CurrentPropertyNameContext.Value = null;
            CurrentScenarioContext.Value = null;
            CurrentRuleConfigContext.Value = null;
        }
    }
}
