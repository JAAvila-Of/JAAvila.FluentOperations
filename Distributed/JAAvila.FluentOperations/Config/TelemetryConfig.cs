namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Immutable telemetry configuration. Controls which validation metrics are emitted
/// via <c>System.Diagnostics.Metrics</c>.
/// </summary>
internal class TelemetryConfig
{
    /// <summary>
    /// When <c>true</c>, telemetry metrics are emitted. Defaults to <c>false</c>.
    /// All other flags are only meaningful when this is <c>true</c>.
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// When <c>true</c>, the duration of each eager rule execution is recorded as a histogram.
    /// </summary>
    public bool TrackRuleExecutionTime { get; init; }

    /// <summary>
    /// When <c>true</c>, failure rate counters are emitted for eager rule executions.
    /// </summary>
    public bool TrackFailureRates { get; init; }

    /// <summary>
    /// When <c>true</c>, the total duration of each <c>Check</c> / <c>CheckAsync</c> call is
    /// recorded as a histogram.
    /// </summary>
    public bool TrackBlueprintExecutionTime { get; init; }
}
