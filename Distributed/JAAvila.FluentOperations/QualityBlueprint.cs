using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Manager;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Telemetry;

// ReSharper disable once RedundantUsingDirective (kept for clarity with System.DateTime/TimeSpan overloads)

namespace JAAvila.FluentOperations;

/// <summary>
/// Base class for defining reusable validation schemas (Quality Blueprints) for a model of type <typeparamref name="T"/>.
/// Subclasses override the constructor to define validation rules using <c>For</c> overloads,
/// then validate instances via <see cref="Check(T)"/> or <see cref="CheckAsync(T)"/>.
/// </summary>
/// <typeparam name="T">The model type to validate.</typeparam>
/// <remarks>
/// Blueprints are designed to be stateless and reusable. Rules are defined once in the constructor
/// and replayed against each model instance during <see cref="Check(T)"/>/<see cref="CheckAsync(T)"/>.
/// </remarks>
/// <example>
/// <code>
/// public class UserBlueprint : QualityBlueprint&lt;User&gt;
/// {
///     public UserBlueprint()
///     {
///         For(x =&gt; x.Name).Test().NotBeNullOrEmpty();
///         For(x =&gt; x.Age).Test().BeGreaterThan(0).BeLessThan(150);
///     }
/// }
///
/// var report = new UserBlueprint().Check(user);
/// if (!report.IsValid) { /* handle failures */ }
/// </code>
/// </example>
public abstract partial class QualityBlueprint<T> : IBlueprintValidator
    where T : notnull
{
    private record RuleDefinition(
        string PropertyName,
        Func<T, object?> ValueExtractor,
        List<IQualityRule> Rules,
        Type? Scenario = null,
        RuleConfig? Config = null
    );

    private record ForEachDefinition(
        string PropertyName,
        Func<T, System.Collections.IEnumerable?> CollectionExtractor,
        List<IQualityRule> Rules,
        ISubBlueprintRule? SubBlueprint,
        Type? Scenario = null,
        RuleConfig? Config = null,
        Func<object, bool>? Filter = null
    );

    private record NestedDefinition(
        string PropertyName,
        Func<T, object?> ChildExtractor,
        ISubBlueprintRule ChildBlueprint,
        Type? Scenario = null
    );

    private readonly List<RuleDefinition> _ruleDefinitions = [];
    private readonly List<ForEachDefinition> _forEachDefinitions = [];
    private readonly List<NestedDefinition> _nestedDefinitions = [];
    private readonly Dictionary<string, Func<T, object?>> _extractors = new();

    // Tracks property names registered via ForEach so DefinitionScope.Dispose() routes them correctly
    private readonly HashSet<string> _forEachPropertyNames = [];

    // Tracks collection extractors for ForEach properties
    private readonly Dictionary<
        string,
        Func<T, System.Collections.IEnumerable?>
    > _forEachCollectionExtractors = new();

    // Tracks optional per-ForEach config
    private readonly Dictionary<string, RuleConfig?> _forEachConfigs = new();

    // Tracks optional per-ForEach filter (for ForEachWhere)
    private readonly Dictionary<string, Func<object, bool>?> _forEachFilters = new();

    private readonly List<IQualityRule> _capturedDuringDefinition = [];
    private readonly List<object> _conditionGroups = []; // ConditionGroup<T> instances for reset
    private T? _instance;
    private Type? _currentScenario;

    /// <summary>
    /// Controls cascade behavior for the entire blueprint.
    /// Defaults to <see cref="Common.CascadeMode.Continue"/> (all rules run even after failures).
    /// </summary>
    /// <remarks>
    /// Set to <see cref="Common.CascadeMode.StopOnFirstFailure"/> to halt validation for a property
    /// as soon as the first rule fails. Per-property cascade can be overridden via <see cref="RuleConfig"/>.
    /// </remarks>
    protected CascadeMode CascadeMode { get; set; } = CascadeMode.Continue;

    /// <summary>
    /// Initializes a new instance of the <see cref="QualityBlueprint{T}"/> class.
    /// </summary>
    protected QualityBlueprint() { }

    /// <summary>
    /// Scopes all rules defined within <paramref name="action"/> to the specified scenario interface.
    /// Rules in a scenario are only evaluated when the validated instance implements <typeparamref name="TInterface"/>.
    /// </summary>
    /// <typeparam name="TInterface">The interface marker that activates this scenario.</typeparam>
    /// <param name="action">An action that defines the rules belonging to this scenario.</param>
    protected void Scenario<TInterface>(Action action)
    {
        var previousScenario = _currentScenario;
        var newScenario = typeof(TInterface);

        _currentScenario = newScenario;

        action();

        _currentScenario = previousScenario;
    }

    /// <summary>
    /// Opens a definition scope that collects rules for a single property capture group.
    /// Dispose the returned scope to commit the captured rules to the blueprint.
    /// </summary>
    /// <returns>An <see cref="IDisposable"/> scope. Dispose to finalize rule registration.</returns>
    protected IDisposable Define()
    {
        var scope = ExecutionEngine<ObjectOperationsManager, object?>.StartCollecting(
            _capturedDuringDefinition
        );
        return new DefinitionScope(this, scope);
    }

    private class DefinitionScope : IDisposable
    {
        private readonly QualityBlueprint<T> _owner;
        private readonly IDisposable _inner;

        public DefinitionScope(QualityBlueprint<T> owner, IDisposable inner)
        {
            _owner = owner;
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
            ExecutionEngine<ObjectOperationsManager, object?>.EndPropertyCapture();

            var groupedRules = _owner
                ._capturedDuringDefinition.OfType<CapturedRule>()
                .GroupBy(cr => new { cr.PropertyName, cr.Scenario });

            foreach (var group in groupedRules)
            {
                var prop = group.Key.PropertyName;
                var scenario = group.Key.Scenario;

                // Route to ForEachDefinitions if this property was registered via ForEach
                if (_owner._forEachPropertyNames.Contains(prop))
                {
                    if (
                        _owner._forEachCollectionExtractors.TryGetValue(
                            prop,
                            out var collectionExtractor
                        )
                    )
                    {
                        var rules = group.Cast<IQualityRule>().ToList();
                        _owner._forEachConfigs.TryGetValue(prop, out var forEachConfig);
                        _owner._forEachFilters.TryGetValue(prop, out var forEachFilter);
                        _owner._forEachDefinitions.Add(
                            new ForEachDefinition(
                                prop,
                                collectionExtractor,
                                rules,
                                null,
                                scenario,
                                forEachConfig,
                                forEachFilter
                            )
                        );
                    }
                    continue;
                }

                if (!_owner._extractors.TryGetValue(prop, out var extractor))
                {
                    continue;
                }

                // Preserve CapturedRule (with RuleConfig) rather than unwrapping to Inner,
                // so that GetSeverity/GetErrorCode/GetCustomMessage can be read in Check().
                var ruleList = group.Cast<IQualityRule>().ToList();

                // Extract definition-level config from the first captured rule (if any).
                // This carries the CascadeMode set via For(expr, config).
                var definitionConfig = group.FirstOrDefault()?.Config;

                _owner._ruleDefinitions.Add(
                    new RuleDefinition(
                        prop,
                        x => extractor(x),
                        ruleList,
                        scenario,
                        definitionConfig
                    )
                );
            }

            _owner._capturedDuringDefinition.Clear();
        }
    }

    private void ResetConditionGroups()
    {
        foreach (var groupObj in _conditionGroups)
        {
            if (groupObj is ConditionGroup<T> group)
            {
                group.Reset();
            }
        }
    }

    /// <summary>
    /// Asynchronously validates the specified instance against all rules defined in this blueprint.
    /// Use this overload when the blueprint contains async custom validators or async predicates defined via <see cref="ForAsync{TProp}(System.Linq.Expressions.Expression{System.Func{T,TProp}},System.Func{TProp,System.Threading.Tasks.Task{bool}},string)"/>.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public async Task<QualityReport> CheckAsync(T instance)
    {
        if (instance == null)
        {
            return new QualityReport();
        }

        var registeredScenarios = _ruleDefinitions
            .Select(d => d.Scenario)
            .Where(s => s != null)
            .Distinct();

        var instanceType = instance.GetType();
        var activeScenario = registeredScenarios.FirstOrDefault(
            s => s!.IsAssignableFrom(instanceType)
        );

        return await CheckAsync(instance, activeScenario);
    }

    /// <summary>
    /// Asynchronously validates the specified instance using only the rules that belong to <paramref name="activeScenario"/>.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="activeScenario">The scenario type to activate, or <c>null</c> to run all non-scenario rules.</param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public async Task<QualityReport> CheckAsync(T instance, Type? activeScenario)
    {
        var telemetryConfig = GlobalConfig.GetTelemetryConfig();
        var telemetryEnabled = telemetryConfig is { Enabled: true };
        var sw = FluentOperationsMeter.StartTimingIfEnabled(telemetryEnabled && telemetryConfig!.TrackBlueprintExecutionTime);

        ResetConditionGroups();
        _instance = instance;
        var report = new QualityReport();


        using (
            new TransactionalOperations(
                "Blueprint CheckAsync",
                TransactionalMode.AccumulateFailsAndDisposeThis
            )
        )
        {
            // Normal property definitions
            foreach (var def in _ruleDefinitions)
            {
                if (def.Scenario != null)
                {
                    if (activeScenario == null || def.Scenario != activeScenario)
                    {
                        continue;
                    }
                }

                report.RulesEvaluated++;

                var currentValue = def.ValueExtractor(instance);

                // Determine effective cascade mode for this definition
                var effectiveCascade = def.Config?.CascadeMode ?? CascadeMode;
                var stopped = false;

                foreach (var rule in def.Rules)
                {
                    if (stopped)
                    {
                        break;
                    }

                    var innerRule = rule is CapturedRule cr ? cr.Inner : rule;

                    // Inject a model instance for conditional rules
                    if (innerRule is ConditionalRuleWrapper<T> conditional)
                    {
                        conditional.SetModelInstance(instance);
                    }

                    // For cross-property rules, pass the full model instance
                    rule.SetValue(innerRule is ICrossPropertyRule ? instance : currentValue);

                    bool isValid;

                    if (innerRule is IAsyncQualityRule)
                    {
                        isValid = await rule.ValidateAsync();
                    }
                    else
                    {
                        isValid = rule.Validate();
                    }

                    if (isValid)
                    {
                        continue;
                    }

                    var customMessage = rule.GetCustomMessage();
                    report.Failures.Add(
                        new QualityFailure
                        {
                            PropertyName = def.PropertyName,
                            Message = customMessage ?? rule.GetReport(),
                            AttemptedValue = currentValue,
                            Severity = rule.GetSeverity(),
                            ErrorCode = rule.GetErrorCode(),
                        }
                    );

                    if (effectiveCascade == CascadeMode.StopOnFirstFailure)
                    {
                        stopped = true;
                    }
                }
            }

            // ForEach definitions
            foreach (var def in _forEachDefinitions)
            {
                if (def.Scenario != null)
                {
                    if (activeScenario == null || def.Scenario != activeScenario)
                    {
                        continue;
                    }
                }

                var collection = def.CollectionExtractor(instance);

                if (collection == null)
                {
                    continue;
                }

                var effectiveCascade = def.Config?.CascadeMode ?? CascadeMode;
                var index = 0;

                foreach (var item in collection)
                {
                    // Apply filter — skip item but preserve the original index
                    if (def.Filter != null && !def.Filter(item!))
                    {
                        index++;
                        continue;
                    }

                    var indexedName = $"{def.PropertyName}[{index}]";

                    if (def.SubBlueprint != null)
                    {
                        // Sub-blueprint variant: validate item asynchronously
                        var failures = await def.SubBlueprint.GetFailuresAsync(item, indexedName);
                        report.Failures.AddRange(failures);
                        report.RulesEvaluated++;
                    }
                    else
                    {
                        // Captured-rules variant: apply each rule to the item
                        report.RulesEvaluated++;
                        var stopped = false;

                        foreach (var rule in def.Rules)
                        {
                            if (stopped)
                            {
                                break;
                            }

                            rule.SetValue(item);

                            bool isValid;
                            var innerRule = rule is CapturedRule cr ? cr.Inner : rule;

                            if (innerRule is IAsyncQualityRule)
                            {
                                isValid = await rule.ValidateAsync();
                            }
                            else
                            {
                                isValid = rule.Validate();
                            }

                            if (isValid)
                            {
                                continue;
                            }

                            var customMessage = rule.GetCustomMessage();
                            report.Failures.Add(
                                new QualityFailure
                                {
                                    PropertyName = indexedName,
                                    Message = customMessage ?? rule.GetReport(),
                                    AttemptedValue = item,
                                    Severity = rule.GetSeverity(),
                                    ErrorCode = rule.GetErrorCode(),
                                }
                            );

                            if (effectiveCascade == CascadeMode.StopOnFirstFailure)
                            {
                                stopped = true;
                            }
                        }
                    }

                    index++;
                }
            }

            // Nested definitions (single child object)
            foreach (var def in _nestedDefinitions)
            {
                if (def.Scenario != null)
                {
                    if (activeScenario == null || def.Scenario != activeScenario)
                    {
                        continue;
                    }
                }

                var childValue = def.ChildExtractor(instance);

                if (childValue == null)
                {
                    continue;
                }

                var failures = await def.ChildBlueprint.GetFailuresAsync(
                    childValue,
                    def.PropertyName
                );
                report.Failures.AddRange(failures);
                report.RulesEvaluated++;
            }
        }

        if (telemetryEnabled)
        {
            sw?.Stop();
            FluentOperationsMeter.RecordBlueprintCheck(
                GetType().Name,
                typeof(T).Name,
                report.IsValid,
                report.RulesEvaluated,
                report.Errors.Count,
                sw?.Elapsed.TotalMilliseconds ?? 0);
        }

        return report;
    }

    /// <summary>
    /// Validates the specified instance against all rules defined in this blueprint.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public QualityReport Check(T instance)
    {
        if (instance == null)
        {
            return new QualityReport();
        }

        var registeredScenarios = _ruleDefinitions
            .Select(d => d.Scenario)
            .Where(s => s != null)
            .Distinct();

        var instanceType = instance.GetType();
        var activeScenario = registeredScenarios.FirstOrDefault(
            s => s!.IsAssignableFrom(instanceType)
        );

        return Check(instance, activeScenario);
    }

    /// <summary>
    /// Validates the specified instance using only the rules that belong to <paramref name="activeScenario"/>.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="activeScenario">The scenario type to activate, or <c>null</c> to run all non-scenario rules.</param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public QualityReport Check(T instance, Type? activeScenario)
    {
        var telemetryConfig = GlobalConfig.GetTelemetryConfig();
        var telemetryEnabled = telemetryConfig is { Enabled: true };
        var sw = FluentOperationsMeter.StartTimingIfEnabled(telemetryEnabled && telemetryConfig!.TrackBlueprintExecutionTime);

        ResetConditionGroups();
        _instance = instance;
        var report = new QualityReport();

        using (
            new TransactionalOperations(
                "Blueprint Check",
                TransactionalMode.AccumulateFailsAndDisposeThis
            )
        )
        {
            // Normal property definitions
            foreach (var def in _ruleDefinitions)
            {
                if (def.Scenario != null)
                {
                    if (activeScenario == null || def.Scenario != activeScenario)
                    {
                        continue;
                    }
                }

                report.RulesEvaluated++;

                var currentValue = def.ValueExtractor(instance);

                // Determine effective cascade mode for this definition
                var effectiveCascade = def.Config?.CascadeMode ?? CascadeMode;
                var stopped = false;

                foreach (var rule in def.Rules)
                {
                    if (stopped)
                    {
                        break;
                    }

                    var innerRule = rule is CapturedRule cr ? cr.Inner : rule;

                    // Inject a model instance for conditional rules
                    if (innerRule is ConditionalRuleWrapper<T> conditional)
                    {
                        conditional.SetModelInstance(instance);
                    }

                    // For cross-property rules, pass the full model instance
                    rule.SetValue(innerRule is ICrossPropertyRule ? instance : currentValue);

                    if (rule.Validate())
                    {
                        continue;
                    }

                    var customMessage = rule.GetCustomMessage();
                    report.Failures.Add(
                        new QualityFailure
                        {
                            PropertyName = def.PropertyName,
                            Message = customMessage ?? rule.GetReport(),
                            AttemptedValue = currentValue,
                            Severity = rule.GetSeverity(),
                            ErrorCode = rule.GetErrorCode(),
                        }
                    );

                    if (effectiveCascade == CascadeMode.StopOnFirstFailure)
                    {
                        stopped = true;
                    }
                }
            }

            // ForEach definitions
            foreach (var def in _forEachDefinitions)
            {
                if (def.Scenario != null)
                {
                    if (activeScenario == null || def.Scenario != activeScenario)
                    {
                        continue;
                    }
                }

                var collection = def.CollectionExtractor(instance);

                if (collection == null)
                {
                    continue;
                }

                var effectiveCascade = def.Config?.CascadeMode ?? CascadeMode;
                var index = 0;

                foreach (var item in collection)
                {
                    // Apply filter — skip item but preserve the original index
                    if (def.Filter != null && !def.Filter(item!))
                    {
                        index++;
                        continue;
                    }

                    var indexedName = $"{def.PropertyName}[{index}]";

                    if (def.SubBlueprint != null)
                    {
                        // Sub-blueprint variant: validate item synchronously
                        var failures = def.SubBlueprint.GetFailures(item, indexedName);
                        report.Failures.AddRange(failures);
                        report.RulesEvaluated++;
                    }
                    else
                    {
                        // Captured-rules variant: apply each rule to the item
                        report.RulesEvaluated++;
                        var stopped = false;

                        foreach (var rule in def.Rules)
                        {
                            if (stopped)
                            {
                                break;
                            }

                            rule.SetValue(item);

                            if (rule.Validate())
                            {
                                continue;
                            }

                            var customMessage = rule.GetCustomMessage();
                            report.Failures.Add(
                                new QualityFailure
                                {
                                    PropertyName = indexedName,
                                    Message = customMessage ?? rule.GetReport(),
                                    AttemptedValue = item,
                                    Severity = rule.GetSeverity(),
                                    ErrorCode = rule.GetErrorCode(),
                                }
                            );

                            if (effectiveCascade == CascadeMode.StopOnFirstFailure)
                            {
                                stopped = true;
                            }
                        }
                    }

                    index++;
                }
            }

            // Nested definitions (single child object)
            foreach (var def in _nestedDefinitions)
            {
                if (def.Scenario != null)
                {
                    if (activeScenario == null || def.Scenario != activeScenario)
                    {
                        continue;
                    }
                }

                var childValue = def.ChildExtractor(instance);

                if (childValue == null)
                {
                    continue;
                }

                var failures = def.ChildBlueprint.GetFailures(childValue, def.PropertyName);
                report.Failures.AddRange(failures);
                report.RulesEvaluated++;
            }
        }

        if (telemetryEnabled)
        {
            sw?.Stop();
            FluentOperationsMeter.RecordBlueprintCheck(
                GetType().Name,
                typeof(T).Name,
                report.IsValid,
                report.RulesEvaluated,
                report.Errors.Count,
                sw?.Elapsed.TotalMilliseconds ?? 0);
        }

        return report;
    }

    // --- Assert methods ------------------------------------------------

    /// <summary>
    /// Validates the specified instance against all rules defined in this blueprint
    /// and throws if any Error-level failures are found.
    /// Warning and Info failures do not cause an exception.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <exception cref="Exceptions.FluentOperationsException">
    /// Thrown (or the active test framework's assertion exception) when validation produces Error-level failures
    /// and no <see cref="AssertionScope"/> is active.
    /// </exception>
    /// <remarks>
    /// When called inside an <see cref="AssertionScope"/>, failures are accumulated instead of thrown immediately.
    /// Scenario detection works identically to <see cref="Check(T)"/>.
    /// </remarks>
    public void Assert(T instance)
    {
        var report = Check(instance);
        if (report.IsValid) return;
        Handler.ExceptionHandler.Handle(FormatAssertMessage(report));
    }

    /// <summary>
    /// Validates the specified instance using only the rules that belong to <paramref name="activeScenario"/>
    /// and throws if any Error-level failures are found.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="activeScenario">The scenario type to activate, or <c>null</c> to run all non-scenario rules.</param>
    /// <exception cref="Exceptions.FluentOperationsException">
    /// Thrown (or the active test framework's assertion exception) when validation produces Error-level failures
    /// and no <see cref="AssertionScope"/> is active.
    /// </exception>
    public void Assert(T instance, Type? activeScenario)
    {
        var report = Check(instance, activeScenario);
        if (report.IsValid) return;
        Handler.ExceptionHandler.Handle(FormatAssertMessage(report));
    }

    /// <summary>
    /// Asynchronously validates the specified instance against all rules defined in this blueprint
    /// and throws if any Error-level failures are found.
    /// Use this overload when the blueprint contains async custom validators.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <exception cref="Exceptions.FluentOperationsException">
    /// Thrown (or the active test framework's assertion exception) when validation produces Error-level failures
    /// and no <see cref="AssertionScope"/> is active.
    /// </exception>
    /// <remarks>
    /// When called inside an <see cref="AssertionScope"/>, failures are accumulated instead of thrown immediately.
    /// Scenario detection works identically to <see cref="CheckAsync(T)"/>.
    /// </remarks>
    public async Task AssertAsync(T instance)
    {
        var report = await CheckAsync(instance);
        if (report.IsValid) return;
        Handler.ExceptionHandler.Handle(FormatAssertMessage(report));
    }

    /// <summary>
    /// Asynchronously validates the specified instance using only the rules that belong to
    /// <paramref name="activeScenario"/> and throws if any Error-level failures are found.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="activeScenario">The scenario type to activate, or <c>null</c> to run all non-scenario rules.</param>
    /// <exception cref="Exceptions.FluentOperationsException">
    /// Thrown (or the active test framework's assertion exception) when validation produces Error-level failures
    /// and no <see cref="AssertionScope"/> is active.
    /// </exception>
    public async Task AssertAsync(T instance, Type? activeScenario)
    {
        var report = await CheckAsync(instance, activeScenario);
        if (report.IsValid) return;
        Handler.ExceptionHandler.Handle(FormatAssertMessage(report));
    }

    /// <summary>
    /// Builds a human-readable failure message from the Error-level failures in the report.
    /// Each failure is formatted using <see cref="QualityFailure.ToString()"/> which includes
    /// severity prefix, property name, message, and optional error code.
    /// </summary>
    private static string FormatAssertMessage(QualityReport report)
    {
        var errors = report.Errors;
        var failures = errors.Select(f => $"  {f}");
        return $"Blueprint validation failed with {errors.Count} error(s):\n"
            + string.Join("\n", failures);
    }

    bool IBlueprintValidator.CanValidate(Type modelType)
    {
        ArgumentNullException.ThrowIfNull(modelType);
        return typeof(T).IsAssignableFrom(modelType);
    }

    QualityReport IBlueprintValidator.Validate(object instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        return Check((T)instance);
    }

    async Task<QualityReport> IBlueprintValidator.ValidateAsync(object instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        return await CheckAsync((T)instance);
    }

    private static string GetPropertyName<TTarget>(Expression<Func<T, TTarget>> expression)
    {
        return expression.Body switch
        {
            MemberExpression member => member.Member.Name,
            UnaryExpression { Operand: MemberExpression member2 } => member2.Member.Name,
            _ => "Property"
        };
    }
}
