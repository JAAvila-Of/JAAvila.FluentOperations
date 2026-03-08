namespace JAAvila.FluentOperations.Common;

/// <summary>
/// Controls the behavior of rule execution when a validation failure occurs for a property.
/// </summary>
public enum CascadeMode
{
    /// <summary>
    /// All validation rules are executed, even after a failure. This is the default.
    /// </summary>
    Continue,

    /// <summary>
    /// Rule execution stops after the first failure for a property or collection item.
    /// Subsequent rules in the same definition are skipped.
    /// </summary>
    StopOnFirstFailure
}
