using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Manager;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

public abstract partial class QualityBlueprint<T>
    where T : notnull
{
    /// <summary>
    /// The property name used in <see cref="QualityFailure.PropertyName"/> for root-model validation rules
    /// defined via <see cref="ForSelf()"/> or <see cref="ForSelfAsync"/>.
    /// </summary>
    public const string RootPropertyName = "(root)";

    /// <summary>
    /// Selects the root model object for validation using object-level operations
    /// (Be, NotBe, BeEquivalentTo, BeAssignableTo, etc.).
    /// Failures are reported with <see cref="RootPropertyName"/> as the property name.
    /// </summary>
    /// <returns>A property proxy that exposes <see cref="ObjectOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ObjectOperationsManager> ForSelf()
    {
        const string propertyName = RootPropertyName;
        _extractors[propertyName] = x => x;

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario,
            currentRuleSet
        );
        RuleCaptureContext.SetRuleConfig(null);

        return new PropertyProxy<object?, ObjectOperationsManager>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller => new ObjectOperationsManager(null, caller),
            () => currentRuleSet
        );
    }

    /// <summary>
    /// Selects the root model object for validation using object-level operations, with a <see cref="RuleConfig"/>.
    /// Failures are reported with <see cref="RootPropertyName"/> as the property name.
    /// </summary>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    /// <returns>A property proxy that exposes <see cref="ObjectOperationsManager"/> via <c>.Test()</c>.</returns>
    protected IPropertyProxy<ObjectOperationsManager> ForSelf(RuleConfig config)
    {
        const string propertyName = RootPropertyName;
        _extractors[propertyName] = x => x;

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;
        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            propertyName,
            currentScenario,
            currentRuleSet
        );
        RuleCaptureContext.SetRuleConfig(config);

        return new PropertyProxy<object?, ObjectOperationsManager>(
            propertyName,
            _capturedDuringDefinition,
            () => currentScenario,
            caller => new ObjectOperationsManager(null, caller),
            () => currentRuleSet
        );
    }

    /// <summary>
    /// Defines a synchronous model-level validation rule using a predicate over the entire root object.
    /// Use this for cross-property business invariants (e.g., total must equal sum of items).
    /// Failures are reported with <see cref="RootPropertyName"/> as the property name.
    /// </summary>
    /// <param name="predicate">A function that receives the model and returns <c>true</c> if validation passes.</param>
    /// <param name="failureMessage">The message to include in the report when the predicate returns <c>false</c>.</param>
    protected void ForSelf(Func<T, bool> predicate, string failureMessage)
    {
        const string propertyName = RootPropertyName;
        _extractors[propertyName] = x => x;

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;
        var rule = new ModelPredicateRule<T>(predicate, failureMessage);
        _capturedDuringDefinition.Add(new CapturedRule(propertyName, rule, currentScenario, null, currentRuleSet));
    }

    /// <summary>
    /// Defines a synchronous model-level validation rule using a predicate over the entire root object,
    /// with a <see cref="RuleConfig"/> that controls severity, error code, and cascade behavior.
    /// Failures are reported with <see cref="RootPropertyName"/> as the property name.
    /// </summary>
    /// <param name="predicate">A function that receives the model and returns <c>true</c> if validation passes.</param>
    /// <param name="failureMessage">The message to include in the report when the predicate returns <c>false</c>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    protected void ForSelf(
        Func<T, bool> predicate,
        string failureMessage,
        RuleConfig config)
    {
        const string propertyName = RootPropertyName;
        _extractors[propertyName] = x => x;

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;
        var rule = new ModelPredicateRule<T>(predicate, failureMessage, config);
        _capturedDuringDefinition.Add(new CapturedRule(propertyName, rule, currentScenario, config, currentRuleSet));
    }

    /// <summary>
    /// Defines an asynchronous model-level validation rule using a predicate over the entire root object.
    /// Use <see cref="CheckAsync(T)"/> to execute blueprints containing async self rules.
    /// Failures are reported with <see cref="RootPropertyName"/> as the property name.
    /// </summary>
    /// <param name="predicate">An async function that receives the model and returns <c>true</c> if validation passes.</param>
    /// <param name="failureMessage">The message to include in the report when the predicate returns <c>false</c>.</param>
    protected void ForSelfAsync(Func<T, Task<bool>> predicate, string failureMessage)
    {
        const string propertyName = RootPropertyName;
        _extractors[propertyName] = x => x;

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;
        var rule = new AsyncModelPredicateRule<T>(predicate, failureMessage);
        _capturedDuringDefinition.Add(new CapturedRule(propertyName, rule, currentScenario, null, currentRuleSet));
    }

    /// <summary>
    /// Defines an asynchronous model-level validation rule using a predicate over the entire root object,
    /// with a <see cref="RuleConfig"/> that controls severity, error code, and cascade behavior.
    /// Use <see cref="CheckAsync(T)"/> to execute blueprints containing async self rules.
    /// Failures are reported with <see cref="RootPropertyName"/> as the property name.
    /// </summary>
    /// <param name="predicate">An async function that receives the model and returns <c>true</c> if validation passes.</param>
    /// <param name="failureMessage">The message to include in the report when the predicate returns <c>false</c>.</param>
    /// <param name="config">Configuration for this rule (severity, error code, cascade mode).</param>
    protected void ForSelfAsync(
        Func<T, Task<bool>> predicate,
        string failureMessage,
        RuleConfig config)
    {
        const string propertyName = RootPropertyName;
        _extractors[propertyName] = x => x;

        var currentScenario = _currentScenario;
        var currentRuleSet = _currentRuleSet;
        var rule = new AsyncModelPredicateRule<T>(predicate, failureMessage, config);
        _capturedDuringDefinition.Add(new CapturedRule(propertyName, rule, currentScenario, config, currentRuleSet));
    }
}
