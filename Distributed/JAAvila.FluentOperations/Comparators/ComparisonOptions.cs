namespace JAAvila.FluentOperations.Comparators;

/// <summary>
/// Configurable options for comparison operations.
/// Designed to be extensible for future comparators (Object, Collection).
/// </summary>
public record ComparisonOptions
{
    /// <summary>
    /// The string comparison mode to use. Default: Ordinal.
    /// </summary>
    public StringComparison StringComparison { get; init; } = StringComparison.Ordinal;

    /// <summary>
    /// Whether to ignore leading whitespace when comparing strings.
    /// </summary>
    public bool IgnoreLeadingWhitespace { get; init; }

    /// <summary>
    /// Whether to ignore trailing whitespace when comparing strings.
    /// </summary>
    public bool IgnoreTrailingWhitespace { get; init; }

    /// <summary>
    /// Whether to normalize newline styles (CRLF vs LF) before comparing.
    /// </summary>
    public bool IgnoreNewLineStyle { get; init; }

    /// <summary>
    /// Maximum recursion depth for deep comparisons (used by future ObjectComparator).
    /// </summary>
    public int MaxRecursionDepth { get; init; } = 10;

    /// <summary>
    /// Properties to exclude from comparison (used by future ObjectComparator).
    /// </summary>
    public HashSet<string> ExcludedProperties { get; init; } = [];

    /// <summary>
    /// Maximum number of differences to report before stopping comparison.
    /// Used by ObjectComparator to limit verbose output. Default: 5.
    /// </summary>
    public int MaxDifferencesReported { get; init; } = 5;

    /// <summary>
    /// Predefined options for case-insensitive ordinal comparison.
    /// </summary>
    public static readonly ComparisonOptions CaseInsensitive = new()
    {
        StringComparison = StringComparison.OrdinalIgnoreCase
    };

    /// <summary>
    /// Default options (ordinal, case-sensitive).
    /// </summary>
    public static readonly ComparisonOptions Default = new();
}
