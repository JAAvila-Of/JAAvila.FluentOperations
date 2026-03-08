namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Represents the result produced by executing a <see cref="QualityBlueprint{T}"/>.
/// Aggregates all validation failures across properties and exposes filtered views by severity.
/// </summary>
public class QualityReport
{
    /// <summary>
    /// Returns true if there are no Error-level failures.
    /// Warning and Info failures do NOT make the report invalid.
    /// </summary>
    public bool IsValid => Failures.All(f => f.Severity != Severity.Error);

    /// <summary>
    /// The complete list of failures recorded during blueprint execution.
    /// </summary>
    public List<QualityFailure> Failures { get; } = [];

    /// <summary>
    /// The total number of rules that were evaluated during blueprint execution.
    /// </summary>
    public int RulesEvaluated { get; set; }

    /// <summary>
    /// Returns true if there is at least one Error-level failure.
    /// </summary>
    public bool HasErrors => Failures.Any(f => f.Severity == Severity.Error);

    /// <summary>
    /// Returns true if there is at least one Warning-level failure.
    /// </summary>
    public bool HasWarnings => Failures.Any(f => f.Severity == Severity.Warning);

    /// <summary>
    /// Returns true if there is at least one Info-level failure.
    /// </summary>
    public bool HasInfos => Failures.Any(f => f.Severity == Severity.Info);

    /// <summary>
    /// Returns all Error-level failures.
    /// </summary>
    public IReadOnlyList<QualityFailure> Errors =>
        Failures.Where(f => f.Severity == Severity.Error).ToList();

    /// <summary>
    /// Returns all Warning-level failures.
    /// </summary>
    public IReadOnlyList<QualityFailure> Warnings =>
        Failures.Where(f => f.Severity == Severity.Warning).ToList();

    /// <summary>
    /// Returns all Info-level failures.
    /// </summary>
    public IReadOnlyList<QualityFailure> Infos =>
        Failures.Where(f => f.Severity == Severity.Info).ToList();

    /// <summary>
    /// Groups failures by property name.
    /// </summary>
    public IReadOnlyDictionary<string, List<QualityFailure>> FailuresByProperty() =>
        Failures.GroupBy(f => f.PropertyName).ToDictionary(g => g.Key, g => g.ToList());

    /// <summary>
    /// Groups failures by severity level.
    /// </summary>
    public IReadOnlyDictionary<Severity, List<QualityFailure>> FailuresBySeverity() =>
        Failures.GroupBy(f => f.Severity).ToDictionary(g => g.Key, g => g.ToList());

    /// <summary>
    /// Returns a summary string with Unicode indicators.
    /// Example: "INVALID: 2 error(s), 1 warning(s) in 5 rules evaluated"
    /// </summary>
    public override string ToString()
    {
        var errorCount = Failures.Count(f => f.Severity == Severity.Error);
        var warningCount = Failures.Count(f => f.Severity == Severity.Warning);
        var infoCount = Failures.Count(f => f.Severity == Severity.Info);
        var status = IsValid ? "VALID" : "INVALID";
        var parts = new List<string>();

        if (errorCount > 0)
        {
            parts.Add($"{errorCount} error(s)");
        }

        if (warningCount > 0)
        {
            parts.Add($"{warningCount} warning(s)");
        }

        if (infoCount > 0)
        {
            parts.Add($"{infoCount} info(s)");
        }

        var failureSummary = parts.Count > 0 ? string.Join(", ", parts) : "no issues";

        return $"{status}: {failureSummary} in {RulesEvaluated} rules evaluated";
    }
}
