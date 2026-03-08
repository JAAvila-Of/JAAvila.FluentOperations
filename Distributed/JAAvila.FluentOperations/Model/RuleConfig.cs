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
    /// Creates a RuleConfig with the specified severity.
    /// </summary>
    public static RuleConfig WithSeverity(Severity severity) => new() { Severity = severity };

    /// <summary>
    /// Creates a RuleConfig with the specified error code.
    /// </summary>
    public static RuleConfig WithCode(string errorCode) => new() { ErrorCode = errorCode };
}
