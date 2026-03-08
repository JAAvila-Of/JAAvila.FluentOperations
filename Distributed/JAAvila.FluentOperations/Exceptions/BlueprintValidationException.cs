using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Exceptions;

/// <summary>
/// Exception thrown when blueprint validation fails in a decorated service.
/// </summary>
public class BlueprintValidationException : Exception
{
    /// <summary>
    /// The full quality report that caused this exception, containing all recorded failures.
    /// </summary>
    public QualityReport Report { get; }

    /// <summary>
    /// Initializes a new instance with the specified error message and the report that triggered the exception.
    /// </summary>
    /// <param name="message">The message that describes the validation failure.</param>
    /// <param name="report">The quality report containing the detailed failure list.</param>
    public BlueprintValidationException(string message, QualityReport report)
        : base(message)
    {
        Report = report;
    }

    /// <summary>
    /// Returns the exception message followed by a formatted list of all property failures.
    /// </summary>
    public override string ToString()
    {
        var failures = string.Join(
            "\n",
            Report.Failures.Select(f => $"  - {f.PropertyName}: {f.Message}")
        );
        return $"{Message}\n{failures}";
    }
}
