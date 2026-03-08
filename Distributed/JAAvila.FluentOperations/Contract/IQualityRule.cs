using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Defines the contract for a quality rule that can validate a value and report failures.
/// Implemented by all captured rules inside a <see cref="QualityBlueprint{T}"/>.
/// </summary>
public interface IQualityRule
{
    /// <summary>
    /// Executes the rule synchronously.
    /// </summary>
    /// <returns><c>true</c> if the rule passes; otherwise <c>false</c>.</returns>
    bool Validate();

    /// <summary>
    /// Executes the rule asynchronously.
    /// </summary>
    /// <returns>A task whose result is <c>true</c> if the rule passes; otherwise <c>false</c>.</returns>
    Task<bool> ValidateAsync();

    /// <summary>
    /// Returns a formatted failure message produced after a failed <see cref="Validate"/> or <see cref="ValidateAsync"/> call.
    /// </summary>
    /// <returns>The human-readable failure description.</returns>
    string GetReport();

    /// <summary>
    /// Injects the property value extracted from the model instance before the rule is evaluated.
    /// </summary>
    /// <param name="value">The property value to validate.</param>
    void SetValue(object? value);

    /// <summary>
    /// Returns the severity level of this rule. Defaults to Error.
    /// </summary>
    Severity GetSeverity() => Severity.Error;

    /// <summary>
    /// Returns the error code associated with this rule, if any.
    /// </summary>
    string? GetErrorCode() => null;

    /// <summary>
    /// Returns the custom error message for this rule, if any.
    /// When set, this message replaces the generated template.
    /// </summary>
    string? GetCustomMessage() => null;
}
