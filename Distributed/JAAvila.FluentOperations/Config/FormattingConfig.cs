namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Configuration for the value formatting pipeline.
/// </summary>
public class FormattingConfig
{
    /// <summary>
    /// Display text for null values. Default: "&lt;null&gt;".
    /// </summary>
    public string NullDisplay { get; init; } = "<null>";

    /// <summary>
    /// Display text for empty strings. Default: "\"\" (&lt;empty&gt;)".
    /// </summary>
    public string EmptyDisplay { get; init; } = "\"\" (<empty>)";

    /// <summary>
    /// Maximum number of collection items to display before truncating.
    /// Default: 10.
    /// </summary>
    public int MaxCollectionItems { get; init; } = 10;

    /// <summary>
    /// Maximum recursion depth for nested object formatting.
    /// Default: 3.
    /// </summary>
    public int MaxDepth { get; init; } = 3;
}
