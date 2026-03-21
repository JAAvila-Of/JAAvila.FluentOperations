using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Snapshot;

/// <summary>
/// Compares a stored <see cref="SnapshotSerializer.SnapshotData"/> against the current <see cref="QualityReport"/>
/// and returns a structured <see cref="SnapshotDiff"/>.
/// </summary>
internal static class SnapshotComparer
{
    /// <summary>
    /// Compares <paramref name="stored"/> against <paramref name="current"/> and returns all detected differences.
    /// </summary>
    internal static SnapshotDiff Compare(
        SnapshotSerializer.SnapshotData stored,
        QualityReport current,
        string snapshotName,
        SnapshotOptions options)
    {
        IsValidChange? isValidChange = stored.IsValid != current.IsValid
            ? new IsValidChange(stored.IsValid, current.IsValid)
            : null;

        RulesEvaluatedChange? rulesEvaluatedChange = stored.RulesEvaluated != current.RulesEvaluated
            ? new RulesEvaluatedChange(stored.RulesEvaluated, current.RulesEvaluated)
            : null;

        // Build lookup from stored failures by composite key (PropertyName, ErrorCode, Message)
        // Multiple failures can share the same key — track which stored entries have been matched.
        var storedLookup = BuildLookup(stored.Failures, options);
        var currentLookup = BuildCurrentLookup(current.Failures, options);

        var added = new List<QualityFailure>();
        var removed = new List<QualityFailure>();
        var changed = new List<FailureChange>();

        // Find added and changed failures (present in current but different or absent in stored)
        foreach (var (key, currentFailure) in currentLookup)
        {
            if (!storedLookup.TryGetValue(key, out var storedFailure))
            {
                added.Add(currentFailure);
                continue;
            }

            // Mark stored entry as matched by removing it
            storedLookup.Remove(key);

            // Check for field-level differences
            var severityChange = CompareSeverity(storedFailure, currentFailure);
            var messageChange = options.IncludeMessages
                ? CompareMessage(storedFailure, currentFailure)
                : null;
            var errorCodeChange = CompareErrorCode(storedFailure, currentFailure);

            if (severityChange is not null || messageChange is not null || errorCodeChange is not null)
            {
                changed.Add(new FailureChange
                {
                    PropertyName = currentFailure.PropertyName,
                    ErrorCode = currentFailure.ErrorCode,
                    SeverityChange = severityChange,
                    MessageChange = messageChange,
                    ErrorCodeChange = errorCodeChange,
                });
            }
        }

        // Any remaining stored entries were not matched — they were removed
        foreach (var (_, storedFailure) in storedLookup)
        {
            removed.Add(new QualityFailure
            {
                PropertyName = storedFailure.PropertyName,
                Message = storedFailure.Message ?? string.Empty,
                Severity = ParseSeverity(storedFailure.Severity),
                ErrorCode = storedFailure.ErrorCode,
            });
        }

        return new SnapshotDiff
        {
            SnapshotName = snapshotName,
            IsValidChange = isValidChange,
            RulesEvaluatedChange = rulesEvaluatedChange,
            AddedFailures = added,
            RemovedFailures = removed,
            ChangedFailures = changed,
        };
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static string BuildKey(string propertyName, string? errorCode, string? message, SnapshotOptions options)
    {
        var messagePart = options.IncludeMessages ? (message ?? string.Empty) : string.Empty;
        return string.Join("|", propertyName, errorCode ?? string.Empty, messagePart);
    }

    private static Dictionary<string, SnapshotSerializer.SnapshotFailure> BuildLookup(
        List<SnapshotSerializer.SnapshotFailure> failures, SnapshotOptions options)
    {
        // Use a counter-based suffix to handle duplicate keys
        var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        var result = new Dictionary<string, SnapshotSerializer.SnapshotFailure>(StringComparer.Ordinal);

        foreach (var f in failures)
        {
            var baseKey = BuildKey(f.PropertyName, f.ErrorCode, f.Message, options);
            counts.TryGetValue(baseKey, out var count);
            var uniqueKey = count == 0 ? baseKey : $"{baseKey}#{count}";
            counts[baseKey] = count + 1;
            result[uniqueKey] = f;
        }

        return result;
    }

    private static Dictionary<string, QualityFailure> BuildCurrentLookup(
        List<QualityFailure> failures, SnapshotOptions options)
    {
        var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        var result = new Dictionary<string, QualityFailure>(StringComparer.Ordinal);

        foreach (var f in failures)
        {
            var baseKey = BuildKey(f.PropertyName, f.ErrorCode, f.Message, options);
            counts.TryGetValue(baseKey, out var count);
            var uniqueKey = count == 0 ? baseKey : $"{baseKey}#{count}";
            counts[baseKey] = count + 1;
            result[uniqueKey] = f;
        }

        return result;
    }

    private static (Severity Expected, Severity Actual)? CompareSeverity(
        SnapshotSerializer.SnapshotFailure stored, QualityFailure current)
    {
        var storedSeverity = ParseSeverity(stored.Severity);
        return storedSeverity != current.Severity
            ? (storedSeverity, current.Severity)
            : null;
    }

    private static (string Expected, string Actual)? CompareMessage(
        SnapshotSerializer.SnapshotFailure stored, QualityFailure current)
    {
        var storedMessage = stored.Message ?? string.Empty;
        return !string.Equals(storedMessage, current.Message, StringComparison.Ordinal)
            ? (storedMessage, current.Message)
            : null;
    }

    private static (string? Expected, string? Actual)? CompareErrorCode(
        SnapshotSerializer.SnapshotFailure stored, QualityFailure current)
    {
        return !string.Equals(stored.ErrorCode, current.ErrorCode, StringComparison.Ordinal)
            ? (stored.ErrorCode, current.ErrorCode)
            : null;
    }

    private static Severity ParseSeverity(string value)
    {
        return Enum.TryParse<Severity>(value, ignoreCase: true, out var result)
            ? result
            : Severity.Error;
    }
}
