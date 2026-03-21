namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Extension methods for <see cref="QualityReport"/> that provide RFC 7807-compatible output formats.
/// </summary>
public static class QualityReportExtensions
{
    /// <summary>
    /// Converts the report into an error dictionary grouped by property name,
    /// compatible with the Problem Details RFC 7807 <c>errors</c> structure.
    /// <para>
    /// Only <see cref="Severity.Error"/>-level failures are included.
    /// Warning and Info failures are excluded from the output.
    /// </para>
    /// </summary>
    /// <param name="report">The quality report to convert.</param>
    /// <returns>
    /// A dictionary where keys are property names and values are arrays of error messages
    /// for that property. Returns an empty dictionary if there are no Error-level failures.
    /// </returns>
    public static IDictionary<string, string[]> ToErrorDictionary(this QualityReport report)
    {
        ArgumentNullException.ThrowIfNull(report);

        return report.Failures
            .Where(f => f.Severity == Severity.Error)
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.Message).ToArray());
    }
}
