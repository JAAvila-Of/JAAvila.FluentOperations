using JAAvila.FluentOperations.Model;
using System.Text;

namespace JAAvila.FluentOperations.Snapshot;

/// <summary>
/// Records a single changed failure: the same key exists in both snapshots but one or more fields differ.
/// </summary>
internal class FailureChange
{
    /// <summary>The property name that identifies this failure (shared between stored and current).</summary>
    public string PropertyName { get; init; } = string.Empty;

    /// <summary>The error code that identifies this failure (shared between stored and current).</summary>
    public string? ErrorCode { get; init; }

    /// <summary>Non-null when the severity changed. Item1 = stored, Item2 = current.</summary>
    public (Severity Expected, Severity Actual)? SeverityChange { get; init; }

    /// <summary>Non-null when the message changed. Item1 = stored, Item2 = current.</summary>
    public (string Expected, string Actual)? MessageChange { get; init; }

    /// <summary>Non-null when the error code changed. Item1 = stored, Item2 = current.</summary>
    public (string? Expected, string? Actual)? ErrorCodeChange { get; init; }
}

/// <summary>
/// Captures the structural differences found between a stored snapshot and the current <see cref="QualityReport"/>.
/// </summary>
internal class SnapshotDiff
{
    /// <summary>The name of the snapshot being compared.</summary>
    public string SnapshotName { get; init; } = string.Empty;

    /// <summary>Non-null when the <c>isValid</c> flag changed.</summary>
    public IsValidChange? IsValidChange { get; init; }

    /// <summary>Non-null when the <c>rulesEvaluated</c> count changed.</summary>
    public RulesEvaluatedChange? RulesEvaluatedChange { get; init; }

    /// <summary>Failures present in the current report but absent from the stored snapshot.</summary>
    public List<QualityFailure> AddedFailures { get; init; } = [];

    /// <summary>Failures present in the stored snapshot but absent from the current report.</summary>
    public List<QualityFailure> RemovedFailures { get; init; } = [];

    /// <summary>Failures that exist in both but have one or more field differences.</summary>
    public List<FailureChange> ChangedFailures { get; init; } = [];

    /// <summary>
    /// Returns <c>true</c> when there are no differences of any kind.
    /// </summary>
    public bool IsMatch =>
        IsValidChange is null &&
        RulesEvaluatedChange is null &&
        AddedFailures.Count == 0 &&
        RemovedFailures.Count == 0 &&
        ChangedFailures.Count == 0;

    /// <summary>
    /// Produces a human-readable diff report for use in assertion failure messages.
    /// </summary>
    public string FormatReport()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Snapshot '{SnapshotName}' does not match the current QualityReport.");
        sb.AppendLine();

        if (IsValidChange is not null)
        {
            sb.AppendLine($"  IsValid changed: expected '{IsValidChange.Expected}', got '{IsValidChange.Actual}'");
        }

        if (RulesEvaluatedChange is not null)
        {
            sb.AppendLine($"  RulesEvaluated changed: expected '{RulesEvaluatedChange.Expected}', got '{RulesEvaluatedChange.Actual}'");
        }

        if (AddedFailures.Count > 0)
        {
            sb.AppendLine($"  Added failures ({AddedFailures.Count}):");
            foreach (var f in AddedFailures)
            {
                sb.AppendLine($"    + [{f.Severity}] {f.PropertyName}: {f.Message}" +
                    (f.ErrorCode is not null ? $" (Code: {f.ErrorCode})" : ""));
            }
        }

        if (RemovedFailures.Count > 0)
        {
            sb.AppendLine($"  Removed failures ({RemovedFailures.Count}):");
            foreach (var f in RemovedFailures)
            {
                sb.AppendLine($"    - [{f.Severity}] {f.PropertyName}: {f.Message}" +
                    (f.ErrorCode is not null ? $" (Code: {f.ErrorCode})" : ""));
            }
        }

        if (ChangedFailures.Count > 0)
        {
            sb.AppendLine($"  Changed failures ({ChangedFailures.Count}):");
            foreach (var c in ChangedFailures)
            {
                sb.AppendLine($"    ~ {c.PropertyName}" + (c.ErrorCode is not null ? $" (Code: {c.ErrorCode})" : "") + ":");
                if (c.SeverityChange is not null)
                    sb.AppendLine($"        Severity: '{c.SeverityChange.Value.Expected}' → '{c.SeverityChange.Value.Actual}'");
                if (c.MessageChange is not null)
                    sb.AppendLine($"        Message:  '{c.MessageChange.Value.Expected}' → '{c.MessageChange.Value.Actual}'");
                if (c.ErrorCodeChange is not null)
                    sb.AppendLine($"        ErrorCode: '{c.ErrorCodeChange.Value.Expected}' → '{c.ErrorCodeChange.Value.Actual}'");
            }
        }

        return sb.ToString().TrimEnd();
    }
}

/// <summary>Captures a change in the <c>isValid</c> flag between snapshot and current report.</summary>
internal record IsValidChange(bool Expected, bool Actual);

/// <summary>Captures a change in the <c>rulesEvaluated</c> count between snapshot and current report.</summary>
internal record RulesEvaluatedChange(int Expected, int Actual);
