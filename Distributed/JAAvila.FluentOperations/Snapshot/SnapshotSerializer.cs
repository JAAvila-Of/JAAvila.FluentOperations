using System.Text.Json;
using System.Text.Json.Serialization;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Snapshot;

/// <summary>
/// Serializes and deserializes <see cref="QualityReport"/> snapshots as deterministic JSON.
/// </summary>
internal static class SnapshotSerializer
{
    private const int CurrentVersion = 1;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    };

    // -------------------------------------------------------------------------
    // Internal models
    // -------------------------------------------------------------------------

    internal class SnapshotData
    {
        [JsonPropertyName("$snapshotVersion")]
        public int SnapshotVersion { get; set; } = CurrentVersion;

        public bool IsValid { get; set; }
        public int RulesEvaluated { get; set; }
        public List<SnapshotFailure> Failures { get; set; } = [];
    }

    internal class SnapshotFailure
    {
        public string PropertyName { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        public string Severity { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AttemptedValue { get; set; }
    }

    // -------------------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------------------

    /// <summary>
    /// Serializes a <see cref="QualityReport"/> to a deterministic JSON string.
    /// Failures are ordered by PropertyName, then Severity, then Message for stability.
    /// </summary>
    internal static string Serialize(QualityReport report, SnapshotOptions options)
    {
        var failures = report.Failures
            .OrderBy(f => f.PropertyName, StringComparer.Ordinal)
            .ThenBy(f => f.Severity.ToString(), StringComparer.Ordinal)
            .ThenBy(f => f.Message, StringComparer.Ordinal)
            .Select(f => new SnapshotFailure
            {
                PropertyName = f.PropertyName,
                Message = options.IncludeMessages ? f.Message : null,
                Severity = f.Severity.ToString(),
                ErrorCode = f.ErrorCode,
                AttemptedValue = options.IncludeAttemptedValues
                    ? f.AttemptedValue?.ToString()
                    : null,
            })
            .ToList();

        var data = new SnapshotData
        {
            SnapshotVersion = CurrentVersion,
            IsValid = report.IsValid,
            RulesEvaluated = report.RulesEvaluated,
            Failures = failures,
        };

        return JsonSerializer.Serialize(data, JsonOptions);
    }

    /// <summary>
    /// Deserializes a JSON string back into a <see cref="SnapshotData"/> instance.
    /// Throws <see cref="InvalidOperationException"/> when the snapshot version is unsupported.
    /// </summary>
    internal static SnapshotData Deserialize(string json)
    {
        var data = JsonSerializer.Deserialize<SnapshotData>(json, JsonOptions)
            ?? throw new InvalidOperationException("Failed to deserialize snapshot: the JSON content is null or empty.");

        if (data.SnapshotVersion != CurrentVersion)
        {
            throw new InvalidOperationException(
                $"Unsupported snapshot version '{data.SnapshotVersion}'. " +
                $"The current supported version is {CurrentVersion}. " +
                "Regenerate the snapshot file by calling UpdateSnapshot().");
        }

        return data;
    }
}
