namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Configuration for string display in error messages.
/// </summary>
public class StringConfig
{
    /// <summary>
    /// Maximum number of characters to display before truncating with ellipsis.
    /// Default: 30. Minimum: 10.
    /// </summary>
    public int MaxDisplayLength { get; init; } = 30;

    /// <summary>
    /// When <c>true</c>, string diff output is appended to failure messages for StartWith and EndWith.
    /// Default: <c>true</c>.
    /// </summary>
    public bool EnableStringDiff { get; init; } = true;

    /// <summary>
    /// Number of characters shown on each side of the first difference in the diff output.
    /// Default: 20. Minimum: 5.
    /// </summary>
    public int StringDiffContextChars { get; init; } = 20;

    /// <summary>
    /// Maximum string length for which diff output is generated.
    /// Strings longer than this receive a truncation notice instead of a full diff.
    /// Default: 1000. Minimum: 50.
    /// </summary>
    public int StringDiffMaxLength { get; init; } = 1000;
}
