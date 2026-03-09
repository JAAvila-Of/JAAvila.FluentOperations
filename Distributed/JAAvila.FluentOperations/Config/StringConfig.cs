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
}
