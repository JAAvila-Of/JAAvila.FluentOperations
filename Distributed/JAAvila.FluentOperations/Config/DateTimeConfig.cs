namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Configuration for date/time value display in error messages.
/// </summary>
public class DateTimeConfig
{
    /// <summary>
    /// Format string for DateTime values. Default: "yyyy-MM-dd HH:mm:ss".
    /// </summary>
    public string Format { get; init; } = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// Format string for DateOnly values. Default: "yyyy-MM-dd".
    /// </summary>
    public string DateOnlyFormat { get; init; } = "yyyy-MM-dd";

    /// <summary>
    /// Format string for TimeOnly values. Default: "HH:mm:ss".
    /// </summary>
    public string TimeOnlyFormat { get; init; } = "HH:mm:ss";

    /// <summary>
    /// Format string for TimeSpan values. Default: null (uses TimeSpan.ToString()).
    /// </summary>
    public string? TimeSpanFormat { get; init; }
}
