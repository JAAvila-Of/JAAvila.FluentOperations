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
    private static readonly AsyncLocal<string?> CurrentRuleSetContext = new();

    /// <summary>Gets the property name currently being captured, or <c>null</c> outside a capture scope.</summary>
    public static string? CurrentPropertyName => CurrentPropertyNameContext.Value;

    /// <summary>Gets the scenario type filter active in the current capture scope, or <c>null</c> if none.</summary>
    public static Type? CurrentScenario => CurrentScenarioContext.Value;

    /// <summary>Gets the <see cref="RuleConfig"/> to attach to the next captured rule, or <c>null</c> if none.</summary>
    public static RuleConfig? CurrentRuleConfig => CurrentRuleConfigContext.Value;

    /// <summary>Gets the rule set name active in the current capture scope, or <c>null</c> if none.</summary>
    public static string? CurrentRuleSet => CurrentRuleSetContext.Value;

    /// <summary>Gets a value indicating whether a rule-collection scope is currently active.</summary>
    public static bool IsCollecting => RulesCollection.Value is not null;

    /// <summary>
    /// Opens a rule-collection scope that directs all subsequent <see cref="AddRule"/> calls into
    /// the specified <paramref name="rules"/> collection. The scope tracks the context including
    /// property name, scenario type, and rule set, ensuring all captured rules are stored with appropriate metadata.
    /// A disposable object is returned to terminate the scope and clear the context.
    /// </summary>
    /// <param name="rules">The list where captured rules will be stored.</param>
    /// <param name="propertyName">Optional property name associated with the scope.</param>
    /// <param name="scenario">Optional scenario type to associate with rules captured in the scope.</param>
    /// <param name="ruleSet">Optional rule set identifier for grouping captured rules.</param>
    /// <returns>An <see cref="IDisposable"/> that ends the collection scope and clears the context upon disposal.</returns>
    public static IDisposable StartCollecting(
        List<IQualityRule> rules,
        string? propertyName = null,
        Type? scenario = null,
        string? ruleSet = null
    )
    {
        RulesCollection.Value = rules;
        CurrentPropertyNameContext.Value = propertyName;
        CurrentScenarioContext.Value = scenario;
        CurrentRuleConfigContext.Value = null;
        CurrentRuleSetContext.Value = ruleSet;
        return new Scope();
    }

    /// <summary>
    /// Initiates a scoped rule capture for a specific property, associating the provided
    /// <paramref name="rules"/> collection, <paramref name="propertyName"/>, and optional
    /// metadata including <paramref name="scenario"/> and <paramref name="ruleSet"/>.
    /// This method does not clear the existing <c>RuleConfig</c>, allowing captured rules to
    /// retain prior configuration when applicable.
    /// </summary>
    /// <param name="rules">The collection that receives the rules captured within the property scope.</param>
    /// <param name="propertyName">The name of the property associated with the captured rules.</param>
    /// <param name="scenario">An optional scenario filter to tag captured rules.</param>
    /// <param name="ruleSet">An optional identifier for grouping captured rules within a specific rule set.</param>
    public static void BeginPropertyCapture(
        List<IQualityRule> rules,
        string propertyName,
        Type? scenario = null,
        string? ruleSet = null
    )
    {
        RulesCollection.Value = rules;
        CurrentPropertyNameContext.Value = propertyName;
        CurrentScenarioContext.Value = scenario;
        CurrentRuleSetContext.Value = ruleSet;
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
    /// Overrides the rule set a name for the current capture scope.
    /// </summary>
    /// <param name="ruleSet">The rule set name to set, or <c>null</c> to clear.</param>
    public static void SetRuleSet(string? ruleSet)
    {
        CurrentRuleSetContext.Value = ruleSet;
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
        CurrentRuleSetContext.Value = null;
    }

    /// <summary>
    /// Wraps <paramref name="rule"/> in a <see cref="CapturedRule"/> with the current property name,
    /// scenario, and config, then adds it to the active rules' collection.
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
        var ruleSet = CurrentRuleSet;

        RulesCollection.Value.Add(new CapturedRule(property, rule, scenario, config, ruleSet));
    }

    private class Scope : IDisposable
    {
        public void Dispose()
        {
            RulesCollection.Value = null;
            CurrentPropertyNameContext.Value = null;
            CurrentScenarioContext.Value = null;
            CurrentRuleConfigContext.Value = null;
            CurrentRuleSetContext.Value = null;
        }
    }
}
