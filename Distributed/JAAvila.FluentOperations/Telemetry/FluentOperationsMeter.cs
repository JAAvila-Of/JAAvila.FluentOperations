using System.Diagnostics;
using System.Diagnostics.Metrics;
using JAAvila.FluentOperations.Config;

namespace JAAvila.FluentOperations.Telemetry;

/// <summary>
/// Central metrics hub for FluentOperations. Emits counters and histograms via
/// <c>System.Diagnostics.Metrics</c> — zero external dependencies, consumable by
/// OpenTelemetry, dotnet-counters, and any <see cref="MeterListener"/>.
/// </summary>
internal static class FluentOperationsMeter
{
    private static readonly Meter Meter = new("JAAvila.FluentOperations", "1.0.0");

    // Counters
    private static readonly Counter<long> RulesEvaluated =
        Meter.CreateCounter<long>("fo.rules.evaluated", description: "Total number of rules evaluated.");

    private static readonly Counter<long> RulesFailed =
        Meter.CreateCounter<long>("fo.rules.failed", description: "Total number of rules that failed.");

    // Histograms
    private static readonly Histogram<double> RuleDuration =
        Meter.CreateHistogram<double>("fo.rule.duration", unit: "ms", description: "Duration of individual rule execution in milliseconds.");

    private static readonly Histogram<double> BlueprintDuration =
        Meter.CreateHistogram<double>("fo.blueprint.duration", unit: "ms", description: "Duration of a full blueprint Check() / CheckAsync() call in milliseconds.");

    /// <summary>
    /// Records metrics for a completed blueprint <c>Check</c> / <c>CheckAsync</c> call.
    /// Early-returns when telemetry is disabled (null config or <c>Enabled == false</c>).
    /// </summary>
    internal static void RecordBlueprintCheck(
        string blueprintTypeName,
        string modelTypeName,
        bool isValid,
        int rulesEvaluated,
        int rulesFailed,
        double elapsedMs)
    {
        var config = GlobalConfig.GetTelemetryConfig();
        if (config is not { Enabled: true })
        {
            return;
        }

        var tags = new TagList
        {
            { "blueprint", blueprintTypeName },
            { "model", modelTypeName },
            { "is_valid", isValid.ToString().ToLowerInvariant() }
        };

        RulesEvaluated.Add(rulesEvaluated, tags);

        if (rulesFailed > 0)
        {
            RulesFailed.Add(rulesFailed, tags);
        }

        if (config.TrackBlueprintExecutionTime)
        {
            BlueprintDuration.Record(elapsedMs, tags);
        }
    }

    /// <summary>
    /// Records metrics for a single eager rule execution (inline assertion path).
    /// Early-returns when telemetry is disabled.
    /// </summary>
    internal static void RecordEagerRuleExecution(bool passed, string severity, double elapsedMs)
    {
        var config = GlobalConfig.GetTelemetryConfig();
        if (config is not { Enabled: true })
        {
            return;
        }

        var tags = new TagList
        {
            { "passed", passed.ToString().ToLowerInvariant() },
            { "severity", severity }
        };

        RulesEvaluated.Add(1, tags);

        if (!passed)
        {
            RulesFailed.Add(1, tags);
        }

        if (config.TrackRuleExecutionTime && elapsedMs > 0)
        {
            RuleDuration.Record(elapsedMs, tags);
        }
    }

    /// <summary>
    /// Returns a running <see cref="Stopwatch"/> when <paramref name="needsTiming"/> is <c>true</c>;
    /// otherwise returns <c>null</c> to avoid unnecessary allocations.
    /// </summary>
    internal static Stopwatch? StartTimingIfEnabled(bool needsTiming) =>
        needsTiming ? Stopwatch.StartNew() : null;
}
