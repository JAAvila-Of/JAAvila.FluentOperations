namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Describes a single validation failure produced by a quality rule.
/// Captures the property name, failure message, attempted value, severity, and optional error code.
/// </summary>
public class QualityFailure
{
    /// <summary>
    /// The name of the property that failed validation.
    /// </summary>
    public string PropertyName { get; init; } = string.Empty;

    /// <summary>
    /// The human-readable message describing the validation failure.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// The value that was being validated when the failure occurred.
    /// </summary>
    public object? AttemptedValue { get; init; }

    /// <summary>
    /// The severity level of this failure. Defaults to Error.
    /// </summary>
    public Severity Severity { get; init; } = Severity.Error;

    /// <summary>
    /// Optional error code associated with this failure.
    /// Can be a constant, enum value converted to string, or literal.
    /// </summary>
    public string? ErrorCode { get; init; }

    /// <summary>
    /// Returns a formatted string representation of this failure.
    /// Format: "[X] PropertyName: Message" or "[X] PropertyName: Message (Code: ERR001)"
    /// </summary>
    public override string ToString()
    {
        var prefix = Severity switch
        {
            Severity.Error => "[X]",
            Severity.Warning => "[!]",
            Severity.Info => "[i]",
            _ => "[?]"
        };

        var codeSuffix = ErrorCode is not null ? $" (Code: {ErrorCode})" : "";
        return $"{prefix} {PropertyName}: {Message}{codeSuffix}";
    }
}
