using System.Linq.Expressions;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Handler;
using JAAvila.FluentOperations.Manager;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Telemetry;
using JAAvila.SafeTypes.Extension;

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
/// public class UserBlueprint: QualityBlueprint&lt;User&gt;
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
        RuleConfig? Config = null,
        string? RuleSet = null
    );

    private record ForEachDefinition(
        string PropertyName,
        Func<T, System.Collections.IEnumerable?> CollectionExtractor,
        List<IQualityRule> Rules,
        ISubBlueprintRule? SubBlueprint,
        Type? Scenario = null,
        RuleConfig? Config = null,
        Func<object, bool>? Filter = null,
        string? RuleSet = null
    );

    private record NestedDefinition(
        string PropertyName,
        Func<T, object?> ChildExtractor,
        ISubBlueprintRule ChildBlueprint,
        Type? Scenario = null,
        string? RuleSet = null
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
    private Type? _currentScenario;
    private string? _currentRuleSet;

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
    /// Controls which severity levels can trigger cascade stop for the entire blueprint.
    /// Defaults to <see cref="Common.CascadeSeverityMode.ErrorOnly"/> -- only Error-severity
    /// failures stop cascade, ensuring Warning/Info failures never hide Error rules.
    /// </summary>
    protected CascadeSeverityMode CascadeSeverityMode { get; set; } = CascadeSeverityMode.ErrorOnly;

    /// <summary>
    /// Enables parallel evaluation of independent property definitions in <see cref="CheckAsync(T)"/>.
    /// When <see langword="true"/>, rule definitions for different properties are evaluated concurrently
    /// via <see cref="Task.WhenAll"/>, reducing total latency when validators contain async I/O operations.
    /// Rules within a single property definition are always evaluated sequentially regardless of this setting.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Only benefits blueprints with async custom validators that perform I/O (HTTP calls, database
    /// queries, file system access). For CPU-bound validators, the overhead of task scheduling may
    /// exceed the benefit.
    /// </para>
    /// <para>
    /// Incompatible with blueprint-level <see cref="Common.CascadeMode.StopOnFirstFailure"/>.
    /// An <see cref="InvalidOperationException"/> is thrown at runtime if both are set.
    /// Per-property <see cref="Common.CascadeMode.StopOnFirstFailure"/> (via <see cref="RuleConfig"/>)
    /// is fully compatible — those rules run sequentially within their property definition.
    /// </para>
    /// <para>
    /// The sync <see cref="Check(T)"/> method is always sequential regardless of this setting.
    /// </para>
    /// </remarks>
    protected bool ParallelExecution { get; set; } = false;

    /// <summary>
    /// The name used for rules defined outside any <see cref="RuleSet"/> block.
    /// These rules always execute unless the caller passes explicit rule set names
    /// that do NOT include this constant.
    /// </summary>
    public const string DefaultRuleSet = "default";

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
    /// Scopes all rules defined within <paramref name="rules"/> to the named rule set.
    /// Rules in a rule set are only evaluated when the caller explicitly includes
    /// that rule set name in the Check/CheckAsync overload.
    /// </summary>
    /// <param name="name">
    /// The name of the rule set. Must not be null, empty, or equal to <see cref="DefaultRuleSet"/> (case-insensitive).
    /// </param>
    /// <param name="rules">An action that defines the rules belonging to this rule set.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="name"/> is null, empty, whitespace, or equals
    /// <see cref="DefaultRuleSet"/> (case-insensitive).
    /// </exception>
    protected void RuleSet(string name, Action rules)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("RuleSet name cannot be null or empty.", nameof(name));
        }

        if (name.Equals(DefaultRuleSet, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                $"'{DefaultRuleSet}' is reserved. Do not wrap rules in RuleSet(\"{DefaultRuleSet}\", ...).",
                nameof(name)
            );
        }

        var previousRuleSet = _currentRuleSet;
        _currentRuleSet = name;

        rules();

        _currentRuleSet = previousRuleSet;
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
                .GroupBy(
                    cr =>
                        new
                        {
                            cr.PropertyName,
                            cr.Scenario,
                            cr.RuleSet
                        }
                );

            foreach (var group in groupedRules)
            {
                var prop = group.Key.PropertyName;
                var scenario = group.Key.Scenario;
                var ruleSet = group.Key.RuleSet;

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
                                forEachFilter,
                                ruleSet
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
                        definitionConfig,
                        ruleSet
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
    /// Returns <see langword="true"/> when the definition should be skipped given the active
    /// scenario and the requested rule sets.
    /// </summary>
    private static bool ShouldSkipDefinition(
        Type? defScenario,
        string? defRuleSet,
        Type? activeScenario,
        HashSet<string>? activeRuleSets
    )
    {
        // --- Scenario filter ---
        if (defScenario != null)
        {
            if (activeScenario == null || defScenario != activeScenario)
            {
                return true;
            }
        }

        // --- RuleSet filter ---
        if (activeRuleSets == null)
        {
            // Caller did not request specific rule sets: execute only default rules (no RuleSet).
            return defRuleSet != null;
        }

        // Caller requested specific rule sets.
        if (defRuleSet == null)
        {
            // Default rule: execute only when "default" is in the requested set.
            return !activeRuleSets.Contains(DefaultRuleSet);
        }

        // Named rule: execute only when its name is in the requested set.
        return !activeRuleSets.Contains(defRuleSet);
    }

    /// <summary>
    /// Normalizes the caller-provided rule set names into a <see cref="HashSet{String}"/> with
    /// ordinal-ignorecase comparison, or returns <c>null</c> when no rule sets were specified.
    /// </summary>
    private static HashSet<string>? NormalizeRuleSets(string[] ruleSets)
    {
        if (ruleSets.IsNullOrEmpty())
        {
            return null;
        }

        return new HashSet<string>(ruleSets, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Auto-detects the active scenario for <paramref name="instance"/> by checking which registered
    /// scenario interfaces the instance's runtime type implements.
    /// </summary>
    private Type? DetectScenario(T instance)
    {
        if (instance.IsNull())
        {
            return null;
        }

        var registeredScenarios = _ruleDefinitions
            .Select(d => d.Scenario)
            .Concat(_forEachDefinitions.Select(d => d.Scenario))
            .Concat(_nestedDefinitions.Select(d => d.Scenario))
            .Where(s => s != null)
            .Distinct();

        var instanceType = instance.GetType();
        return registeredScenarios.FirstOrDefault(s => s!.IsAssignableFrom(instanceType));
    }

    /// <summary>
    /// Evaluates a list of rules synchronously against a given value, applying cascade logic,
    /// creating a <see cref="QualityFailure"/> for each failure, and adding them to <paramref name="report"/>.
    /// </summary>
    /// <param name="rules">The rules to evaluate.</param>
    /// <param name="propertyName">The property name to use in each <see cref="QualityFailure"/>.</param>
    /// <param name="value">The value to validate (currentValue for properties, item for ForEach).</param>
    /// <param name="instance">The full model instance. Used only when <paramref name="isPropertyMode"/> is true.</param>
    /// <param name="isPropertyMode">
    ///   When <c>true</c>, activates property-mode behavior:
    ///   (a) injects <paramref name="instance"/> into <see cref="IModelAwareRule"/> rules via SetModelInstance, and
    ///   (b) passes <paramref name="instance"/> as the value for <see cref="ICrossPropertyRule"/> rules instead of <paramref name="value"/>.
    ///   When <c>false</c> (ForEach mode), <paramref name="value"/> is always passed directly.
    /// </param>
    /// <param name="effectiveCascade">The effective <see cref="CascadeMode"/> for this definition.</param>
    /// <param name="effectiveSeverityMode">The effective <see cref="CascadeSeverityMode"/> for this definition.</param>
    /// <param name="report">The report where failures are added. Does NOT increment <c>RulesEvaluated</c> — caller's responsibility.</param>
    private void EvaluateRules(
        List<IQualityRule> rules,
        string propertyName,
        object? value,
        T instance,
        bool isPropertyMode,
        CascadeMode effectiveCascade,
        CascadeSeverityMode effectiveSeverityMode,
        QualityReport report
    )
    {
        Severity? stoppedAtSeverity = null;

        foreach (var rule in rules)
        {
            // --- Block A: Cascade skip ---
            if (stoppedAtSeverity.HasValue)
            {
                if (effectiveSeverityMode == CascadeSeverityMode.SameOrLowerSeverity)
                {
                    // In graduated mode, skip rules at equal or lower severity than the trigger
                    // but allow higher-severity rules to still execute
                    if (rule.GetSeverity() >= stoppedAtSeverity.Value)
                        continue;
                }
                else
                {
                    // ErrorOnly or AllFailures: hard stop on all later rules
                    break;
                }
            }

            // --- Block B: Rule preparation ---
            var innerRule = rule is CapturedRule cr ? cr.Inner : rule;

            // Inject a root model for model-aware rules (dynamic messages via MessageFactory,
            // cross-property conditions, etc.). CapturedRule.SetModelInstance propagates to inner.
            if (rule is IModelAwareRule modelAware)
            {
                modelAware.SetModelInstance(instance);
            }

            if (isPropertyMode)
            {
                // Inject a model instance for inner conditional rules not yet covered by the above
                if (innerRule is IModelAwareRule innerModelAware && rule is not IModelAwareRule)
                {
                    innerModelAware.SetModelInstance(instance);
                }

                // For cross-property rules, pass the full model instance
                rule.SetValue(innerRule is ICrossPropertyRule ? instance : value);
            }
            else
            {
                // ForEach mode: always pass the item directly
                rule.SetValue(value);
            }

            // --- Block C: Sync dispatch ---
            if (rule.Validate())
            {
                continue;
            }

            // --- Block D: Create QualityFailure ---
            // Thread-safe message resolution: invoke MessageFactory with the in-scope instance
            // directly rather than relying on _currentModel stored on CapturedRule (which would
            // be a mutable instance field subject to data races under concurrent Check() calls).
            var customMessage = rule is CapturedRule { Config.MessageFactory: { } factoryForMsg }
                ? factoryForMsg(instance)
                : rule.GetCustomMessage();
            report.Failures.Add(
                new QualityFailure
                {
                    PropertyName = propertyName,
                    Message = customMessage ?? rule.GetReport(),
                    AttemptedValue = value,
                    Severity = rule.GetSeverity(),
                    ErrorCode = rule.GetErrorCode(),
                }
            );

            // --- Block E: Cascade stop ---
            if (effectiveCascade == CascadeMode.StopOnFirstFailure)
            {
                var failedSeverity = rule.GetSeverity();
                var shouldStop = effectiveSeverityMode switch
                {
                    CascadeSeverityMode.ErrorOnly => failedSeverity == Severity.Error,
                    CascadeSeverityMode.AllFailures => true,
                    CascadeSeverityMode.SameOrLowerSeverity => true,
                    _ => failedSeverity == Severity.Error
                };

                if (shouldStop)
                {
                    if (!stoppedAtSeverity.HasValue || failedSeverity < stoppedAtSeverity.Value)
                        stoppedAtSeverity = failedSeverity;
                }
            }
        }
    }

    /// <summary>
    /// Async version of <see cref="EvaluateRules"/>. Identical logic except async dispatch:
    /// rules implementing <see cref="IAsyncQualityRule"/> are awaited via <c>ValidateAsync()</c>.
    /// Does NOT increment <c>RulesEvaluated</c> — caller's responsibility.
    /// </summary>
    private async Task EvaluateRulesAsync(
        List<IQualityRule> rules,
        string propertyName,
        object? value,
        T instance,
        bool isPropertyMode,
        CascadeMode effectiveCascade,
        CascadeSeverityMode effectiveSeverityMode,
        QualityReport report
    )
    {
        Severity? stoppedAtSeverity = null;

        foreach (var rule in rules)
        {
            // --- Block A: Cascade skip ---
            if (stoppedAtSeverity.HasValue)
            {
                if (effectiveSeverityMode == CascadeSeverityMode.SameOrLowerSeverity)
                {
                    // In graduated mode, skip rules at equal or lower severity than the trigger
                    // but allow higher-severity rules to still execute
                    if (rule.GetSeverity() >= stoppedAtSeverity.Value)
                        continue;
                }
                else
                {
                    // ErrorOnly or AllFailures: hard stop on all later rules
                    break;
                }
            }

            // --- Block B: Rule preparation ---
            var innerRule = rule is CapturedRule cr ? cr.Inner : rule;

            // Inject a root model for model-aware rules (dynamic messages via MessageFactory,
            // cross-property conditions, etc.). CapturedRule.SetModelInstance propagates to inner.
            if (rule is IModelAwareRule modelAware)
            {
                modelAware.SetModelInstance(instance);
            }

            if (isPropertyMode)
            {
                // Inject a model instance for inner conditional rules not yet covered by the above
                if (innerRule is IModelAwareRule innerModelAware && rule is not IModelAwareRule)
                {
                    innerModelAware.SetModelInstance(instance);
                }

                // For cross-property rules, pass the full model instance
                rule.SetValue(innerRule is ICrossPropertyRule ? instance : value);
            }
            else
            {
                // ForEach mode: always pass the item directly
                rule.SetValue(value);
            }

            // --- Block C: Async/sync dispatch ---
            bool isValid;

            if (innerRule is IAsyncQualityRule)
            {
                isValid = await rule.ValidateAsync();
            }
            else
            {
                // ReSharper disable once MethodHasAsyncOverload
                isValid = rule.Validate();
            }

            if (isValid)
            {
                continue;
            }

            // --- Block D: Create QualityFailure ---
            // Thread-safe message resolution: invoke MessageFactory with the in-scope instance
            // directly rather than relying on _currentModel stored on CapturedRule (which would
            // be a mutable instance field subject to data races under concurrent CheckAsync() calls).
            var customMessage = rule is CapturedRule { Config.MessageFactory: { } factoryForMsg }
                ? factoryForMsg(instance)
                : rule.GetCustomMessage();
            report.Failures.Add(
                new QualityFailure
                {
                    PropertyName = propertyName,
                    Message = customMessage ?? rule.GetReport(),
                    AttemptedValue = value,
                    Severity = rule.GetSeverity(),
                    ErrorCode = rule.GetErrorCode(),
                }
            );

            // --- Block E: Cascade stop ---
            if (effectiveCascade == CascadeMode.StopOnFirstFailure)
            {
                var failedSeverity = rule.GetSeverity();
                var shouldStop = effectiveSeverityMode switch
                {
                    CascadeSeverityMode.ErrorOnly => failedSeverity == Severity.Error,
                    CascadeSeverityMode.AllFailures => true,
                    CascadeSeverityMode.SameOrLowerSeverity => true,
                    _ => failedSeverity == Severity.Error
                };

                if (shouldStop)
                {
                    if (!stoppedAtSeverity.HasValue || failedSeverity < stoppedAtSeverity.Value)
                        stoppedAtSeverity = failedSeverity;
                }
            }
        }
    }

    // --- CheckAsync overloads ---

    /// <summary>
    /// Asynchronously validates the specified instance against all rules defined in this blueprint.
    /// Use this overload when the blueprint contains async custom validators or async predicates defined via <see cref="ForAsync{TProp}(System.Linq.Expressions.Expression{System.Func{T,TProp}},System.Func{TProp,System.Threading.Tasks.Task{bool}},string)"/>.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public async Task<QualityReport> CheckAsync(T instance)
    {
        if (instance.IsNull())
        {
            return new QualityReport();
        }

        return await CheckAsyncInternal(instance, DetectScenario(instance), activeRuleSets: null);
    }

    /// <summary>
    /// Asynchronously validates the specified instance using only the rules that belong to <paramref name="activeScenario"/>.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="activeScenario">The scenario type to activate, or <c>null</c> to run all non-scenario rules.</param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public async Task<QualityReport> CheckAsync(T instance, Type? activeScenario)
    {
        if (instance.IsNull())
        {
            return new QualityReport();
        }

        return await CheckAsyncInternal(instance, activeScenario, activeRuleSets: null);
    }

    /// <summary>
    /// Asynchronously validates the specified instance against the rules belonging to the specified rule sets.
    /// Default rules (defined outside any <see cref="RuleSet"/> block) are included only when
    /// <see cref="DefaultRuleSet"/> is among the requested names.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="ruleSets">
    /// The rule set names to activate. Pass <see cref="DefaultRuleSet"/> to include unconditional rules.
    /// </param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public async Task<QualityReport> CheckRuleSetsAsync(T instance, params string[] ruleSets)
    {
        if (instance.IsNull())
        {
            return new QualityReport();
        }

        return await CheckAsyncInternal(
            instance,
            DetectScenario(instance),
            NormalizeRuleSets(ruleSets)
        );
    }

    /// <summary>
    /// Asynchronously validates the specified instance using a specific scenario and rule sets.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="activeScenario">The scenario type to activate, or <c>null</c> to run all non-scenario rules.</param>
    /// <param name="ruleSets">
    /// The rule set names to activate. Pass <see cref="DefaultRuleSet"/> to include unconditional rules.
    /// </param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public async Task<QualityReport> CheckRuleSetsAsync(
        T instance,
        Type? activeScenario,
        params string[] ruleSets
    )
    {
        if (instance.IsNull())
        {
            return new QualityReport();
        }

        return await CheckAsyncInternal(instance, activeScenario, NormalizeRuleSets(ruleSets));
    }

    private async Task<QualityReport> CheckAsyncInternal(
        T instance,
        Type? activeScenario,
        HashSet<string>? activeRuleSets
    )
    {
        if (ParallelExecution && CascadeMode == CascadeMode.StopOnFirstFailure)
        {
            throw new InvalidOperationException(
                "ParallelExecution is incompatible with blueprint-level CascadeMode.StopOnFirstFailure. "
                    + "Use per-property RuleConfig.CascadeMode instead, or disable ParallelExecution."
            );
        }

        var telemetryConfig = GlobalConfig.GetTelemetryConfig();
        var telemetryEnabled = telemetryConfig is { Enabled: true };
        var sw = FluentOperationsMeter.StartTimingIfEnabled(
            telemetryEnabled && telemetryConfig!.TrackBlueprintExecutionTime
        );

        ResetConditionGroups();
        var report = new QualityReport();

        using (
            new TransactionalOperations(
                "Blueprint CheckAsync",
                TransactionalMode.AccumulateFailsAndDisposeThis
            )
        )
        {
            if (ParallelExecution)
            {
                await ExecuteParallelAsync(instance, activeScenario, activeRuleSets, report)
                    .ConfigureAwait(false);
            }
            else
            {
                // Sequential path — behavior identical to before refactoring

                // Normal property definitions
                foreach (var def in _ruleDefinitions)
                {
                    if (
                        ShouldSkipDefinition(
                            def.Scenario,
                            def.RuleSet,
                            activeScenario,
                            activeRuleSets
                        )
                    )
                    {
                        continue;
                    }

                    report.RulesEvaluated++;

                    var currentValue = def.ValueExtractor(instance);

                    await EvaluateRulesAsync(
                        def.Rules,
                        def.PropertyName,
                        currentValue,
                        instance,
                        isPropertyMode: true,
                        def.Config?.CascadeMode ?? CascadeMode,
                        def.Config?.CascadeSeverityMode ?? CascadeSeverityMode,
                        report
                    );
                }

                // ForEach definitions
                foreach (var def in _forEachDefinitions)
                {
                    if (
                        ShouldSkipDefinition(
                            def.Scenario,
                            def.RuleSet,
                            activeScenario,
                            activeRuleSets
                        )
                    )
                    {
                        continue;
                    }

                    var collection = def.CollectionExtractor(instance);

                    if (collection == null)
                    {
                        continue;
                    }

                    var effectiveCascade = def.Config?.CascadeMode ?? CascadeMode;
                    var effectiveSeverityMode =
                        def.Config?.CascadeSeverityMode ?? CascadeSeverityMode;
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
                            var failures = await def.SubBlueprint.GetFailuresAsync(
                                item,
                                indexedName
                            );
                            report.Failures.AddRange(failures);
                            report.RulesEvaluated++;
                        }
                        else
                        {
                            // Captured-rules variant: apply each rule to the item
                            report.RulesEvaluated++;
                            await EvaluateRulesAsync(
                                def.Rules,
                                indexedName,
                                item,
                                instance,
                                isPropertyMode: false,
                                effectiveCascade,
                                effectiveSeverityMode,
                                report
                            );
                        }

                        index++;
                    }
                }

                // Nested definitions (single child object)
                foreach (var def in _nestedDefinitions)
                {
                    if (
                        ShouldSkipDefinition(
                            def.Scenario,
                            def.RuleSet,
                            activeScenario,
                            activeRuleSets
                        )
                    )
                    {
                        continue;
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
                sw?.Elapsed.TotalMilliseconds ?? 0
            );
        }

        return report;
    }

    /// <summary>
    /// Evaluates all definitions concurrently, with each definition processed sequentially
    /// in isolation. Used when <see cref="ParallelExecution"/> is <see langword="true"/>.
    /// </summary>
    /// <remarks>
    /// Each definition task creates its own <see cref="TransactionalOperations"/> scope so that
    /// <see cref="ExceptionHandler"/> writes go to a task-local accumulator rather than the shared
    /// parent scope. Failures are merged back into <paramref name="report"/> after
    /// <see cref="Task.WhenAll"/> completes, preserving the original definition order.
    /// </remarks>
    private async Task ExecuteParallelAsync(
        T instance,
        Type? activeScenario,
        HashSet<string>? activeRuleSets,
        QualityReport report
    )
    {
        var tasks = new List<Task<(List<QualityFailure> Failures, int RulesEvaluated)>>();

        // Launch tasks for RuleDefinitions
        foreach (var def in _ruleDefinitions)
        {
            if (ShouldSkipDefinition(def.Scenario, def.RuleSet, activeScenario, activeRuleSets))
            {
                continue;
            }

            tasks.Add(EvaluateRuleDefinitionParallelAsync(def, instance));
        }

        // Launch tasks for ForEachDefinitions
        foreach (var def in _forEachDefinitions)
        {
            if (ShouldSkipDefinition(def.Scenario, def.RuleSet, activeScenario, activeRuleSets))
            {
                continue;
            }

            tasks.Add(EvaluateForEachDefinitionParallelAsync(def, instance));
        }

        // Launch tasks for NestedDefinitions
        foreach (var def in _nestedDefinitions)
        {
            if (ShouldSkipDefinition(def.Scenario, def.RuleSet, activeScenario, activeRuleSets))
            {
                continue;
            }

            tasks.Add(EvaluateNestedDefinitionParallelAsync(def, instance));
        }

        var results = await Task.WhenAll(tasks).ConfigureAwait(false);

        // Merge in original definition order — Task.WhenAll preserves array order
        foreach (var (failures, rulesEvaluated) in results)
        {
            report.Failures.AddRange(failures);
            report.RulesEvaluated += rulesEvaluated;
        }
    }

    /// <summary>
    /// Evaluates a single <see cref="RuleDefinition"/> in isolation, accumulating failures into a
    /// task-local list. The caller is responsible for merging the result into the main report.
    /// </summary>
    private async Task<(
        List<QualityFailure> Failures,
        int RulesEvaluated
    )> EvaluateRuleDefinitionParallelAsync(RuleDefinition def, T instance)
    {
        var localReport = new QualityReport();

        using (
            new TransactionalOperations(
                $"Parallel-{def.PropertyName}",
                TransactionalMode.AccumulateFailsAndDisposeThis
            )
        )
        {
            var currentValue = def.ValueExtractor(instance);

            await EvaluateRulesAsync(
                    def.Rules,
                    def.PropertyName,
                    currentValue,
                    instance,
                    isPropertyMode: true,
                    def.Config?.CascadeMode ?? CascadeMode,
                    def.Config?.CascadeSeverityMode ?? CascadeSeverityMode,
                    localReport
                )
                .ConfigureAwait(false);
        }

        return (localReport.Failures, 1);
    }

    /// <summary>
    /// Evaluates a single <see cref="ForEachDefinition"/> in isolation (all items sequentially),
    /// accumulating failures into a task-local list.
    /// </summary>
    private async Task<(
        List<QualityFailure> Failures,
        int RulesEvaluated
    )> EvaluateForEachDefinitionParallelAsync(ForEachDefinition def, T instance)
    {
        var localReport = new QualityReport();

        using (
            new TransactionalOperations(
                $"Parallel-{def.PropertyName}",
                TransactionalMode.AccumulateFailsAndDisposeThis
            )
        )
        {
            var collection = def.CollectionExtractor(instance);

            if (collection == null)
            {
                return (localReport.Failures, 0);
            }

            var effectiveCascade = def.Config?.CascadeMode ?? CascadeMode;
            var effectiveSeverityMode = def.Config?.CascadeSeverityMode ?? CascadeSeverityMode;
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
                    var failures = await def.SubBlueprint.GetFailuresAsync(item, indexedName)
                        .ConfigureAwait(false);
                    localReport.Failures.AddRange(failures);
                    localReport.RulesEvaluated++;
                }
                else
                {
                    // Captured-rules variant: apply each rule to the item
                    localReport.RulesEvaluated++;
                    await EvaluateRulesAsync(
                            def.Rules,
                            indexedName,
                            item,
                            instance,
                            isPropertyMode: false,
                            effectiveCascade,
                            effectiveSeverityMode,
                            localReport
                        )
                        .ConfigureAwait(false);
                }

                index++;
            }
        }

        return (localReport.Failures, localReport.RulesEvaluated);
    }

    /// <summary>
    /// Evaluates a single <see cref="NestedDefinition"/> in isolation, accumulating failures into
    /// a task-local list.
    /// </summary>
    private async Task<(
        List<QualityFailure> Failures,
        int RulesEvaluated
    )> EvaluateNestedDefinitionParallelAsync(NestedDefinition def, T instance)
    {
        var localReport = new QualityReport();

        using (
            new TransactionalOperations(
                $"Parallel-{def.PropertyName}",
                TransactionalMode.AccumulateFailsAndDisposeThis
            )
        )
        {
            var childValue = def.ChildExtractor(instance);

            if (childValue == null)
            {
                return (localReport.Failures, 0);
            }

            var failures = await def.ChildBlueprint.GetFailuresAsync(childValue, def.PropertyName)
                .ConfigureAwait(false);

            localReport.Failures.AddRange(failures);
        }

        return (localReport.Failures, 1);
    }

    // --- Check overloads ---

    /// <summary>
    /// Validates the specified instance against all rules defined in this blueprint.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public QualityReport Check(T instance)
    {
        if (instance.IsNull())
        {
            return new QualityReport();
        }

        return CheckInternal(instance, DetectScenario(instance), activeRuleSets: null);
    }

    /// <summary>
    /// Validates the specified instance using only the rules that belong to <paramref name="activeScenario"/>.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="activeScenario">The scenario type to activate, or <c>null</c> to run all non-scenario rules.</param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public QualityReport Check(T instance, Type? activeScenario)
    {
        if (instance.IsNull())
        {
            return new QualityReport();
        }

        return CheckInternal(instance, activeScenario, activeRuleSets: null);
    }

    /// <summary>
    /// Validates the specified instance against the rules belonging to the specified rule sets.
    /// Default rules (defined outside any <see cref="RuleSet"/> block) are included only when
    /// <see cref="DefaultRuleSet"/> is among the requested names.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="ruleSets">
    /// The rule set names to activate. Pass <see cref="DefaultRuleSet"/> to include unconditional rules.
    /// </param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public QualityReport CheckRuleSets(T instance, params string[] ruleSets)
    {
        if (instance.IsNull())
        {
            return new QualityReport();
        }

        return CheckInternal(instance, DetectScenario(instance), NormalizeRuleSets(ruleSets));
    }

    /// <summary>
    /// Validates the specified instance using a specific scenario and rule sets.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="activeScenario">The scenario type to activate, or <c>null</c> to run all non-scenario rules.</param>
    /// <param name="ruleSets">
    /// The rule set names to activate. Pass <see cref="DefaultRuleSet"/> to include unconditional rules.
    /// </param>
    /// <returns>A <see cref="QualityReport"/> containing validation results and any failures.</returns>
    public QualityReport CheckRuleSets(T instance, Type? activeScenario, params string[] ruleSets)
    {
        if (instance.IsNull())
        {
            return new QualityReport();
        }

        return CheckInternal(instance, activeScenario, NormalizeRuleSets(ruleSets));
    }

    private QualityReport CheckInternal(
        T instance,
        Type? activeScenario,
        HashSet<string>? activeRuleSets
    )
    {
        var telemetryConfig = GlobalConfig.GetTelemetryConfig();
        var telemetryEnabled = telemetryConfig is { Enabled: true };
        var sw = FluentOperationsMeter.StartTimingIfEnabled(
            telemetryEnabled && telemetryConfig!.TrackBlueprintExecutionTime
        );

        ResetConditionGroups();
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
                if (ShouldSkipDefinition(def.Scenario, def.RuleSet, activeScenario, activeRuleSets))
                {
                    continue;
                }

                report.RulesEvaluated++;

                var currentValue = def.ValueExtractor(instance);

                EvaluateRules(
                    def.Rules,
                    def.PropertyName,
                    currentValue,
                    instance,
                    isPropertyMode: true,
                    def.Config?.CascadeMode ?? CascadeMode,
                    def.Config?.CascadeSeverityMode ?? CascadeSeverityMode,
                    report
                );
            }

            // ForEach definitions
            foreach (var def in _forEachDefinitions)
            {
                if (ShouldSkipDefinition(def.Scenario, def.RuleSet, activeScenario, activeRuleSets))
                {
                    continue;
                }

                var collection = def.CollectionExtractor(instance);

                if (collection == null)
                {
                    continue;
                }

                var effectiveCascade = def.Config?.CascadeMode ?? CascadeMode;
                var effectiveSeverityMode = def.Config?.CascadeSeverityMode ?? CascadeSeverityMode;
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
                        EvaluateRules(
                            def.Rules,
                            indexedName,
                            item,
                            instance,
                            isPropertyMode: false,
                            effectiveCascade,
                            effectiveSeverityMode,
                            report
                        );
                    }

                    index++;
                }
            }

            // Nested definitions (single child object)
            foreach (var def in _nestedDefinitions)
            {
                if (ShouldSkipDefinition(def.Scenario, def.RuleSet, activeScenario, activeRuleSets))
                {
                    continue;
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
                sw?.Elapsed.TotalMilliseconds ?? 0
            );
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

        if (report.IsValid)
        {
            return;
        }

        ExceptionHandler.Handle(FormatAssertMessage(report));
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

        if (report.IsValid)
        {
            return;
        }

        ExceptionHandler.Handle(FormatAssertMessage(report));
    }

    /// <summary>
    /// Validates the specified instance against the rules belonging to the specified rule sets
    /// and throws if any Error-level failures are found.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="ruleSets">The rule set names to activate.</param>
    /// <exception cref="Exceptions.FluentOperationsException">
    /// Thrown (or the active test framework's assertion exception) when validation produces Error-level failures
    /// and no <see cref="AssertionScope"/> is active.
    /// </exception>
    public void AssertRuleSets(T instance, params string[] ruleSets)
    {
        var report = CheckRuleSets(instance, ruleSets);

        if (report.IsValid)
        {
            return;
        }

        ExceptionHandler.Handle(FormatAssertMessage(report));
    }

    /// <summary>
    /// Validates the specified instance using a specific scenario and rule sets,
    /// and throws if any Error-level failures are found.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="activeScenario">The scenario type to activate, or <c>null</c> to run all non-scenario rules.</param>
    /// <param name="ruleSets">The rule set names to activate.</param>
    /// <exception cref="Exceptions.FluentOperationsException">
    /// Thrown (or the active test framework's assertion exception) when validation produces Error-level failures
    /// and no <see cref="AssertionScope"/> is active.
    /// </exception>
    public void AssertRuleSets(T instance, Type? activeScenario, params string[] ruleSets)
    {
        var report = CheckRuleSets(instance, activeScenario, ruleSets);

        if (report.IsValid)
        {
            return;
        }

        ExceptionHandler.Handle(FormatAssertMessage(report));
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

        if (report.IsValid)
        {
            return;
        }

        ExceptionHandler.Handle(FormatAssertMessage(report));
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

        if (report.IsValid)
        {
            return;
        }

        ExceptionHandler.Handle(FormatAssertMessage(report));
    }

    /// <summary>
    /// Asynchronously validates the specified instance against the rules belonging to the specified rule sets
    /// and throws if any Error-level failures are found.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="ruleSets">The rule set names to activate.</param>
    /// <exception cref="Exceptions.FluentOperationsException">
    /// Thrown (or the active test framework's assertion exception) when validation produces Error-level failures
    /// and no <see cref="AssertionScope"/> is active.
    /// </exception>
    public async Task AssertRuleSetsAsync(T instance, params string[] ruleSets)
    {
        var report = await CheckRuleSetsAsync(instance, ruleSets);

        if (report.IsValid)
        {
            return;
        }

        ExceptionHandler.Handle(FormatAssertMessage(report));
    }

    /// <summary>
    /// Asynchronously validates the specified instance using a specific scenario and rule sets,
    /// and throws if any Error-level failures are found.
    /// </summary>
    /// <param name="instance">The model instance to validate.</param>
    /// <param name="activeScenario">The scenario type to activate, or <c>null</c> to run all non-scenario rules.</param>
    /// <param name="ruleSets">The rule set names to activate.</param>
    /// <exception cref="Exceptions.FluentOperationsException">
    /// Thrown (or the active test framework's assertion exception) when validation produces Error-level failures
    /// and no <see cref="AssertionScope"/> is active.
    /// </exception>
    public async Task AssertRuleSetsAsync(
        T instance,
        Type? activeScenario,
        params string[] ruleSets
    )
    {
        var report = await CheckRuleSetsAsync(instance, activeScenario, ruleSets);

        if (report.IsValid)
        {
            return;
        }

        ExceptionHandler.Handle(FormatAssertMessage(report));
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

    QualityReport IBlueprintValidator.Validate(object instance, params string[] ruleSets)
    {
        ArgumentNullException.ThrowIfNull(instance);
        return CheckRuleSets((T)instance, ruleSets);
    }

    async Task<QualityReport> IBlueprintValidator.ValidateAsync(
        object instance,
        params string[] ruleSets
    )
    {
        ArgumentNullException.ThrowIfNull(instance);
        return await CheckRuleSetsAsync((T)instance, ruleSets);
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
