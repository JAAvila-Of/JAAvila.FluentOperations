using JAAvila.FluentOperations.Config;

namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Immutable context passed to each formatter during value formatting.
/// Created from the current configuration.
/// </summary>
public record FormattingContext
{
    /// <summary>
    /// Maximum recursion depth for nested object formatting.
    /// </summary>
    public int MaxDepth { get; init; } = 3;

    /// <summary>
    /// Current recursion depth. Incremented for nested objects.
    /// </summary>
    public int CurrentDepth { get; init; }

    /// <summary>
    /// Maximum string display length before truncation.
    /// </summary>
    public int MaxDisplayLength { get; init; } = 30;

    /// <summary>
    /// Display text for null values.
    /// </summary>
    public string NullDisplay { get; init; } = "<null>";

    /// <summary>
    /// Display text for empty strings.
    /// </summary>
    public string EmptyDisplay { get; init; } = "\"\" (<empty>)";

    /// <summary>
    /// Maximum number of collection items to display before truncating.
    /// </summary>
    public int MaxCollectionItems { get; init; } = 10;

    /// <summary>
    /// Numeric config (decimal places, thousands separator, culture).
    /// </summary>
    public NumericConfig NumericConfig { get; init; } = new();

    /// <summary>
    /// DateTime config (format strings).
    /// </summary>
    public DateTimeConfig DateTimeConfig { get; init; } = new();

    /// <summary>
    /// Creates a FormattingContext from the current GlobalConfig.
    /// </summary>
    internal static FormattingContext FromCurrentConfig()
    {
        var sc = GlobalConfig.GetStringConfig();
        var fc = GlobalConfig.GetFormattingConfig();
        var nc = GlobalConfig.GetNumericConfig();
        var dc = GlobalConfig.GetDateTimeConfig();

        return new FormattingContext
        {
            MaxDepth = fc.MaxDepth,
            CurrentDepth = 0,
            MaxDisplayLength = sc.MaxDisplayLength,
            NullDisplay = fc.NullDisplay,
            EmptyDisplay = fc.EmptyDisplay,
            MaxCollectionItems = fc.MaxCollectionItems,
            NumericConfig = nc,
            DateTimeConfig = dc
        };
    }

    /// <summary>
    /// Returns a new context with incremented depth (for recursive formatting).
    /// </summary>
    public FormattingContext Descend() => this with { CurrentDepth = CurrentDepth + 1 };
}
