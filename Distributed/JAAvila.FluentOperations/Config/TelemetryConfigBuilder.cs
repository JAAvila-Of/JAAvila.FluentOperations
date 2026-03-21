namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Mutable builder for <see cref="TelemetryConfig"/>. Used inside <see cref="FluentOperationsConfig.Configure"/>.
/// </summary>
public class TelemetryConfigBuilder
{
    /// <summary>
    /// Enables or disables telemetry metric emission. Defaults to <c>false</c>.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// When <c>true</c>, the duration of each eager rule execution is recorded.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool TrackRuleExecutionTime { get; set; }

    /// <summary>
    /// When <c>true</c>, failure rate counters are emitted for eager rule executions.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool TrackFailureRates { get; set; }

    /// <summary>
    /// When <c>true</c>, the total duration of each blueprint <c>Check</c> / <c>CheckAsync</c> call is recorded.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool TrackBlueprintExecutionTime { get; set; }
}
