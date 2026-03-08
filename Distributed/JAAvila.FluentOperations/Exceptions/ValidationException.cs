using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Exceptions;

/// <summary>
/// Exception thrown when validation failures are detected.
/// Provides structured access to failure details for programmatic handling.
/// </summary>
public class ValidationException : FluentOperationsException
{
    /// <summary>
    /// The name of the property that failed validation, if applicable.
    /// </summary>
    public string? PropertyName { get; }

    /// <summary>
    /// The value that was being validated when the failure occurred.
    /// </summary>
    public object? AttemptedValue { get; }

    /// <summary>
    /// The list of validation failures. Empty for single inline validations.
    /// </summary>
    public IReadOnlyList<QualityFailure> Failures { get; }

    /// <summary>
    /// Initializes a new instance with the specified error message.
    /// </summary>
    /// <param name="message">The message that describes the validation failure.</param>
    public ValidationException(string message)
        : base(message)
    {
        Failures = [];
    }

    /// <summary>
    /// Initializes a new instance with the specified error message, property name, and attempted value.
    /// Typically used for inline single-property assertions.
    /// </summary>
    /// <param name="message">The message that describes the validation failure.</param>
    /// <param name="propertyName">The name of the property that failed validation.</param>
    /// <param name="attemptedValue">The value that was being validated.</param>
    public ValidationException(string message, string propertyName, object? attemptedValue)
        : base(message)
    {
        PropertyName = propertyName;
        AttemptedValue = attemptedValue;
        Failures = [];
    }

    /// <summary>
    /// Initializes a new instance with the specified error message and a list of structured failures.
    /// Typically used when a blueprint check produces multiple failures.
    /// </summary>
    /// <param name="message">The message that describes the validation failure.</param>
    /// <param name="failures">The list of individual validation failures.</param>
    public ValidationException(string message, IReadOnlyList<QualityFailure> failures)
        : base(message)
    {
        Failures = failures;
    }
}
