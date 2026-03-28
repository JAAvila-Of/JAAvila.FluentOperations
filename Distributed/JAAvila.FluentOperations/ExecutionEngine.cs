using System.Diagnostics;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Handler;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Telemetry;

namespace JAAvila.FluentOperations;

/// <summary>
/// The ExecutionEngine class is responsible for executing validation and handling operations.
/// It provides functionality for attaching an operation, applying a template, and executing both synchronous
/// and asynchronous operations, while delegating exception handling.
/// </summary>
/// <typeparam name="T">The type of the manager implementing ITestManager.</typeparam>
/// <typeparam name="Ts">The type for the principal chain's associated data.</typeparam>
internal class ExecutionEngine<T, Ts>(T manager) : IQualityRule, IRuleDescriptor, IModelAwareRule
    where T : ITestManager<T, Ts>
{
    private IValidator? _operation;
    private Func<TemplateHandler, IValidator, TemplateHandler>? _templateHandler;
    private List<Func<T, (bool, Fail)>>? _conditions;
    private string _template = "";

    private Severity _severity = Severity.Error;
    private string? _errorCode;
    private string? _customMessage;

    private readonly Type? _activeScenarioInChain = RuleCaptureContext.CurrentScenario;

    private static readonly IReadOnlyDictionary<string, object> EmptyParams =
        new Dictionary<string, object>();

    /// <summary>Exposes the attached validator for introspection (e.g., IRuleDescriptor).</summary>
    internal IValidator? Operation => _operation;

    string IRuleDescriptor.OperationName =>
        _operation is IRuleDescriptor rd ? rd.OperationName : (_operation?.MessageKey ?? "Unknown");

    Type IRuleDescriptor.SubjectType =>
        _operation is IRuleDescriptor rd ? rd.SubjectType : typeof(Ts);

    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        _operation is IRuleDescriptor rd ? rd.Parameters : EmptyParams;

    public static bool IsLazyMode { get; set; }

    public static IDisposable StartCollecting(
        List<IQualityRule> rules,
        string? propertyName = null,
        Type? scenario = null
    ) => RuleCaptureContext.StartCollecting(rules, propertyName, scenario);

    public static void BeginPropertyCapture(
        List<IQualityRule> rules,
        string propertyName,
        Type? scenario = null,
        string? ruleSet = null
    ) => RuleCaptureContext.BeginPropertyCapture(rules, propertyName, scenario, ruleSet);

    public static void EndPropertyCapture() => RuleCaptureContext.EndPropertyCapture();

    public static bool IsCollecting => RuleCaptureContext.IsCollecting;

    public static void AddRule(IQualityRule rule) => AddRule(rule, null);

    public static void AddRule(IQualityRule rule, Type? overrideScenario)
    {
        if (!RuleCaptureContext.IsCollecting)
        {
            return;
        }

        var scenario = overrideScenario ?? RuleCaptureContext.CurrentScenario;
        RuleCaptureContext.AddRule(rule, scenario);
    }

    private class RulesCollectorScope : IDisposable
    {
        public void Dispose() { }
    }

    /// <summary>
    /// Creates a new <see cref="ExecutionEngine{T, TS}"/> instance bound to the given manager.
    /// </summary>
    /// <param name="manager">The operations manager that owns this execution engine.</param>
    /// <returns>A new <see cref="ExecutionEngine{T, TS}"/> instance.</returns>
    public static ExecutionEngine<T, Ts> New(T manager) => new(manager);

    /// <summary>
    /// Attaches the validator that will be executed when <see cref="Execute"/> or <see cref="ExecuteAsync"/> is called.
    /// </summary>
    /// <typeparam name="TOperation">The concrete validator type.</typeparam>
    /// <param name="operation">The validator instance to attach.</param>
    /// <returns>The current engine instance for fluent chaining.</returns>
    public ExecutionEngine<T, Ts> WithOperation<TOperation>(TOperation operation)
        where TOperation : IValidator
    {
        _operation = operation;

        return this;
    }

    /// <summary>
    /// Registers a factory function that builds the failure message template from the
    /// <see cref="TemplateHandler"/> and the attached validator.
    /// </summary>
    /// <param name="template">
    /// A delegate that receives a <see cref="TemplateHandler"/> and the active <see cref="IValidator"/>
    /// and returns the configured handler whose <c>Result</c> will be used as the failure message.
    /// </param>
    /// <returns>The current engine instance for fluent chaining.</returns>
    public ExecutionEngine<T, Ts> WithTemplate(
        Func<TemplateHandler, IValidator, TemplateHandler> template
    )
    {
        _templateHandler = template;

        return this;
    }

    /// <summary>
    /// Registers a precondition that, when it evaluates to <c>true</c>, short-circuits the
    /// validator and immediately reports the associated <see cref="Fail"/> message.
    /// Multiple calls to <c>FailIf</c> are evaluated in registration order; the first match wins.
    /// </summary>
    /// <param name="condition">
    /// A delegate that receives the manager and returns a tuple of (<c>bool</c> shouldFail, <see cref="Fail"/> details).
    /// </param>
    /// <returns>The current engine instance for fluent chaining.</returns>
    public ExecutionEngine<T, Ts> FailIf(Func<T, (bool, Fail)> condition)
    {
        _conditions ??= new List<Func<T, (bool, Fail)>>(1);
        _conditions.Add(condition);
        return this;
    }

    /// <summary>
    /// Sets the severity level for this validation rule.
    /// In eager mode: Error throws, Warning, and Info are silently ignored.
    /// In Blueprint mode: all severities are captured in the report.
    /// </summary>
    public ExecutionEngine<T, Ts> WithSeverity(Severity severity)
    {
        _severity = severity;
        return this;
    }

    /// <summary>
    /// Associates an error code with this validation rule.
    /// The code will be included in QualityFailure when used in Blueprint mode.
    /// </summary>
    public ExecutionEngine<T, Ts> WithErrorCode(string errorCode)
    {
        _errorCode = errorCode;
        return this;
    }

    /// <summary>
    /// Sets a custom error message that replaces the generated template.
    /// Useful for user-facing messages that differ from the technical validation message.
    /// </summary>
    public ExecutionEngine<T, Ts> WithCustomMessage(string message)
    {
        _customMessage = message;
        return this;
    }

    /// <summary>
    /// Executes the validation synchronously.
    /// In lazy/collecting mode, the rule is captured for deferred Blueprint evaluation instead.
    /// In eager mode, preconditions are checked first; if the validator fails and severity is
    /// <see cref="Severity.Error"/>, an exception is thrown via <see cref="ExceptionHandler"/>.
    /// </summary>
    public void Execute()
    {
        if (IsLazyMode || IsCollecting)
        {
            AddRule(this, _activeScenarioInChain);
            return;
        }

        var telemetryConfig = GlobalConfig.GetTelemetryConfig();
        var sw =
            (telemetryConfig is { Enabled: true, TrackRuleExecutionTime: true })
                ? Stopwatch.StartNew()
                : null;

        var fail = EnsureFailConditions();

        if (fail)
        {
            if (telemetryConfig is { Enabled: true })
            {
                sw?.Stop();
                FluentOperationsMeter.RecordEagerRuleExecution(
                    false,
                    _severity.ToString(),
                    sw?.Elapsed.TotalMilliseconds ?? 0
                );
            }

            // In eager mode, only Error severity throws
            if (_severity == Severity.Error)
            {
                ExceptionHandler.Handle(_customMessage ?? _template);
            }

            // Warning and Info are silently ignored in eager mode for FailIf conditions
            return;
        }

        var result = BaseOperations.SafeExecute(() => _operation!.Validate());

        if (telemetryConfig is { Enabled: true })
        {
            sw?.Stop();
            FluentOperationsMeter.RecordEagerRuleExecution(
                result,
                _severity.ToString(),
                sw?.Elapsed.TotalMilliseconds ?? 0
            );
        }

        if (result)
        {
            return;
        }

        if (_severity == Severity.Error)
        {
            ExceptionHandler.Handle(_customMessage ?? GetTemplate());
        }

        // Warning and Info: validation failed, but severity is not Error, so no exception
    }

    /// <summary>
    /// Validates the rule synchronously as part of Blueprint execution.
    /// Returns <c>false</c> if a precondition fires or the underlying validator fails.
    /// </summary>
    bool IQualityRule.Validate()
    {
        var fail = EnsureFailConditions();

        return !fail && BaseOperations.SafeExecute(() => _operation!.Validate());
    }

    /// <summary>
    /// Validates the rule asynchronously as part of Blueprint execution.
    /// Returns <c>false</c> if a precondition fires or the underlying validator fails.
    /// </summary>
    async Task<bool> IQualityRule.ValidateAsync()
    {
        var fail = EnsureFailConditions();

        if (fail)
        {
            return false;
        }

        return await BaseOperations.SafeExecuteAsync(() => _operation!.ValidateAsync());
    }

    /// <summary>
    /// Returns the failure message for this rule.
    /// Returns the custom message if one is set; otherwise builds and returns the template string.
    /// </summary>
    string IQualityRule.GetReport()
    {
        if (_customMessage is not null)
        {
            return _customMessage;
        }

        return !string.IsNullOrEmpty(_template) ? _template : GetTemplate();
    }

    /// <summary>
    /// Injects the model property value into the manager's <see cref="PrincipalChain{T}"/>
    /// so that the captured validator operates on the real instance during Blueprint execution.
    /// Null is only accepted for reference types.
    /// </summary>
    /// <param name="value">The property value extracted from the model being validated.</param>
    void IQualityRule.SetValue(object? value)
    {
        switch (value)
        {
            case Ts typedValue:
                manager.PrincipalChain.ReInitialize(typedValue);
                break;
            case null when !typeof(Ts).IsValueType:
                manager.PrincipalChain.ReInitialize(default!);
                break;
        }
    }

    Severity IQualityRule.GetSeverity() => _severity;

    string? IQualityRule.GetErrorCode() => _errorCode;

    string? IQualityRule.GetCustomMessage() => _customMessage;

    /// <summary>
    /// Receives the root model instance from the Blueprint evaluation loop.
    /// The model is not used by ExecutionEngine directly — MessageFactory resolution
    /// is handled by the enclosing <see cref="CapturedRule"/> decorator.
    /// </summary>
    void IModelAwareRule.SetModelInstance(object model)
    {
        // No-op: ExecutionEngine does not use the root model.
        // MessageFactory resolution occurs in CapturedRule.GetCustomMessage().
    }

    /// <summary>
    /// Executes the validation asynchronously.
    /// In lazy/collecting mode, the rule is captured for deferred Blueprint evaluation instead.
    /// In eager mode, preconditions are checked first; if the validator fails, an exception is thrown.
    /// </summary>
    public async Task ExecuteAsync()
    {
        if (IsLazyMode || IsCollecting)
        {
            AddRule(this, _activeScenarioInChain);
            return;
        }

        var telemetryConfig = GlobalConfig.GetTelemetryConfig();
        var sw =
            (telemetryConfig is { Enabled: true, TrackRuleExecutionTime: true })
                ? Stopwatch.StartNew()
                : null;

        var fail = EnsureFailConditions();

        if (fail)
        {
            if (telemetryConfig is { Enabled: true })
            {
                sw?.Stop();
                FluentOperationsMeter.RecordEagerRuleExecution(
                    false,
                    _severity.ToString(),
                    sw?.Elapsed.TotalMilliseconds ?? 0
                );
            }

            // In eager mode, only Error severity throws
            if (_severity == Severity.Error)
            {
                ExceptionHandler.Handle(_customMessage ?? _template);
            }

            // Warning and Info are silently ignored in eager mode for FailIf conditions
            return;
        }

        var result = await BaseOperations.SafeExecuteAsync(() => _operation!.ValidateAsync());

        if (telemetryConfig is { Enabled: true })
        {
            sw?.Stop();
            FluentOperationsMeter.RecordEagerRuleExecution(
                result,
                _severity.ToString(),
                sw?.Elapsed.TotalMilliseconds ?? 0
            );
        }

        if (!result)
        {
            ExceptionHandler.Handle(GetTemplate());
        }
    }

    #region PRIVATE METHODS

    private bool EnsureFailConditions()
    {
        if (_conditions is null || _conditions.Count == 0)
        {
            return false;
        }

        foreach (var condition in _conditions)
        {
            var result = condition(manager);

            if (!result.Item1)
            {
                continue;
            }

            _template = new TemplateHandler()
                .WithSubject(manager.PrincipalChain.GetSubject())
                .WithFail(result.Item2.Because, result.Item2.Arguments)
                .Result;

            return true;
        }

        return false;
    }

    private string GetTemplate()
    {
        if (_templateHandler is not null)
        {
            var handler = _templateHandler(new TemplateHandler(), _operation!);
            handler.WithSeverity(_severity);
            handler.WithErrorCode(_errorCode);
            return handler.Result;
        }

        var fallback = new TemplateHandler()
            .WithSubject(manager.PrincipalChain.GetSubject())
            .WithExpected(_operation!.Expected);

        fallback.WithSeverity(_severity);
        fallback.WithErrorCode(_errorCode);

        return fallback.Result;
    }

    #endregion
}
