namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Defines the severity level of a validation rule.
/// </summary>
public enum Severity
{
    /// <summary>
    /// The validation failure is an error. This is the default severity.
    /// In eager mode, this causes an exception to be thrown.
    /// In Blueprint mode, this makes IsValid return false.
    /// </summary>
    Error = 0,

    /// <summary>
    /// The validation failure is a warning.
    /// In eager mode, this is logged but does not throw.
    /// In Blueprint mode, this is captured but does not affect IsValid.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// The validation failure is informational.
    /// In eager mode, this is a no-op.
    /// In Blueprint mode, this is captured but does not affect IsValid.
    /// </summary>
    Info = 2,
}
