namespace JAAvila.FluentOperations.Common;

/// <summary>
/// Controls which severity levels trigger cascade stop behavior when
/// <see cref="CascadeMode.StopOnFirstFailure"/> is active.
/// </summary>
public enum CascadeSeverityMode
{
    /// <summary>
    /// Only Error-severity failures trigger cascade stop. Warning and Info failures
    /// are recorded but do not prevent subsequent rules from executing.
    /// This is the default, ensuring Error-level rules are never silently skipped.
    /// </summary>
    ErrorOnly,

    /// <summary>
    /// All failures trigger cascade stop regardless of severity.
    /// A Warning or Info failure will prevent subsequent rules (including Error rules)
    /// from running on the same property.
    /// </summary>
    AllFailures,

    /// <summary>
    /// A failure stops subsequent rules of the same or lower severity.
    /// Error (0) stops all; Warning (1) stops Warning and Info; Info (2) stops only Info.
    /// Higher-severity rules always execute.
    /// </summary>
    SameOrLowerSeverity
}
