using JAAvila.FluentOperations.Common;

namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Configuration options for a validation rule in a Blueprint.
/// </summary>
public record RuleConfig
{
    /// <summary>
    /// The severity level of the rule. Defaults to Error.
    /// </summary>
    public Severity Severity { get; init; } = Severity.Error;

    /// <summary>
    /// Optional error code for the rule.
    /// </summary>
    public string? ErrorCode { get; init; }

    /// <summary>
    /// Optional custom error message that replaces the generated template.
    /// </summary>
    public string? CustomMessage { get; init; }

    /// <summary>
    /// Optional factory function that generates a dynamic error message using the root model instance.
    /// When set, this takes precedence over <see cref="CustomMessage"/>.
    /// The factory receives the model as <c>object</c> — cast to your model type inside the lambda.
    /// </summary>
    /// <remarks>
    /// The factory is invoked only during Blueprint <c>Check()</c>/<c>CheckAsync()</c> evaluation,
    /// when a rule fails. It is NOT invoked in inline (eager) mode where no root model is available.
    /// </remarks>
    /// <example>
    /// <code>
    /// For(x => x.Email, new RuleConfig
    /// {
    ///     MessageFactory = model => $"Email for {((Order)model).CustomerName} is invalid"
    /// }).Test().BeEmail();
    /// </code>
    /// </example>
    public Func<object, string>? MessageFactory { get; init; }

    /// <summary>
    /// Creates a new RuleConfig with default values (Severity.Error, no code, no message).
    /// </summary>
    public static RuleConfig Default => new();

    /// <summary>
    /// Controls rule cascade behavior for this specific rule or property definition.
    /// When set to StopOnFirstFailure, validation stops after the first failure.
    /// When null, the blueprint-level CascadeMode applies.
    /// </summary>
    public CascadeMode? CascadeMode { get; init; }

    /// <summary>
    /// Controls which failure severities trigger cascade stop for this property definition.
    /// When null, the blueprint-level CascadeSeverityMode applies.
    /// Defaults to null (inherit from blueprint).
    /// </summary>
    public CascadeSeverityMode? CascadeSeverityMode { get; init; }

    /// <summary>
    /// Creates a RuleConfig with the specified severity.
    /// </summary>
    public static RuleConfig WithSeverity(Severity severity) => new() { Severity = severity };

    /// <summary>
    /// Creates a RuleConfig with the specified error code.
    /// </summary>
    public static RuleConfig WithCode(string errorCode) => new() { ErrorCode = errorCode };
}
