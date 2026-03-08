using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Provides advanced formatting options for QualityReport output.
/// </summary>
internal static class QualityReportFormatter
{
    /// <summary>
    /// Formats a QualityReport into a detailed multi-line string with Unicode indicators.
    /// </summary>
    public static string Format(QualityReport report, bool includeAttemptedValues = false)
    {
        var sb = new System.Text.StringBuilder();

        // Header
        sb.AppendLine(report.ToString());
        sb.AppendLine(new string('-', 60));

        if (report.Failures.Count == 0)
        {
            sb.AppendLine("  No issues found.");
            return sb.ToString();
        }

        // Group by property
        var byProperty = report.FailuresByProperty();

        foreach (var kvp in byProperty)
        {
            sb.AppendLine($"  {kvp.Key}:");

            foreach (var failure in kvp.Value)
            {
                var prefix = failure.Severity switch
                {
                    Severity.Error => "[X]",
                    Severity.Warning => "[!]",
                    Severity.Info => "[i]",
                    _ => "[?]"
                };

                var codePart = failure.ErrorCode is not null ? $" [{failure.ErrorCode}]" : "";
                sb.AppendLine($"    {prefix}{codePart} {failure.Message}");

                if (includeAttemptedValues && failure.AttemptedValue is not null)
                {
                    sb.AppendLine($"        Attempted: {failure.AttemptedValue}");
                }
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Formats a QualityReport as a JSON-like structure for structured logging.
    /// </summary>
    public static string FormatAsStructured(QualityReport report)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("{");
        sb.AppendLine($"  \"isValid\": {report.IsValid.ToString().ToLowerInvariant()},");
        sb.AppendLine($"  \"rulesEvaluated\": {report.RulesEvaluated},");
        sb.AppendLine($"  \"errors\": {report.Errors.Count},");
        sb.AppendLine($"  \"warnings\": {report.Warnings.Count},");
        sb.AppendLine($"  \"infos\": {report.Infos.Count},");
        sb.AppendLine("  \"failures\": [");

        for (var i = 0; i < report.Failures.Count; i++)
        {
            var f = report.Failures[i];
            var comma = i < report.Failures.Count - 1 ? "," : "";
            sb.AppendLine("    {");
            sb.AppendLine($"      \"property\": \"{EscapeJson(f.PropertyName)}\",");
            sb.AppendLine($"      \"severity\": \"{f.Severity}\",");
            sb.AppendLine($"      \"message\": \"{EscapeJson(f.Message)}\",");
            sb.AppendLine(
                $"      \"errorCode\": {(f.ErrorCode is not null ? $"\"{EscapeJson(f.ErrorCode)}\"" : "null")},"
            );
            sb.AppendLine(
                $"      \"attemptedValue\": {(f.AttemptedValue is not null ? $"\"{EscapeJson(f.AttemptedValue.ToString() ?? "")}\"" : "null")}"
            );
            sb.AppendLine($"    }}{comma}");
        }

        sb.AppendLine("  ]");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private static string EscapeJson(string value) =>
        value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t")
            .Replace("\b", "\\b")
            .Replace("\f", "\\f");
}
