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
    /// Whether to ignore element order when comparing collections.
    /// When <c>true</c>, collections are compared as unordered bags (same elements, any order).
    /// When <c>false</c> (default), collections are compared index-by-index.
    /// Used by ObjectComparator when comparing nested collections inside deep comparison.
    /// </summary>
    public bool IgnoreCollectionOrder { get; init; }

    /// <summary>
    /// Per-type numeric tolerances for approximate comparison.
    /// Key is the compared type (e.g., typeof(decimal), typeof(double), typeof(DateTime)),
    /// value is the tolerance (decimal for decimal, double for double, TimeSpan for DateTime).
    /// When set, values within the tolerance are considered equal.
    /// </summary>
    public IReadOnlyDictionary<Type, object>? Tolerances { get; init; }

    /// <summary>
    /// Member name mappings: source property name -> target property name.
    /// When matching properties on the expected object, uses the mapped name instead.
    /// Enables cross-type comparison where actual and expected have different property names for the same data.
    /// </summary>
    public IReadOnlyDictionary<string, string>? MemberMappings { get; init; }

    /// <summary>
    /// Predefined options for case-insensitive ordinal comparison.
    /// </summary>
    public static readonly ComparisonOptions CaseInsensitive = new()
    {
        StringComparison = StringComparison.OrdinalIgnoreCase
    };

    /// <summary>
    /// Predefined options that ignore element order in nested collections.
    /// </summary>
    public static readonly ComparisonOptions IgnoreOrder = new()
    {
        IgnoreCollectionOrder = true
    };

    /// <summary>
    /// Default options (ordinal, case-sensitive).
    /// </summary>
    public static readonly ComparisonOptions Default = new();
}
