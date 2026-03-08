using System.Globalization;

namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Configuration for numeric value display in error messages.
/// </summary>
public class NumericConfig
{
    /// <summary>
    /// Number of decimal places to display for floating-point values.
    /// Default: -1 (use ToString() without formatting, preserving current behavior).
    /// Set to 2, 4, etc. to format with fixed decimal places.
    /// </summary>
    public int DecimalPlaces { get; init; } = -1;

    /// <summary>
    /// Whether to use thousands separator when displaying numeric values.
    /// Default: false (preserving current behavior).
    /// </summary>
    public bool UseThousandsSeparator { get; init; }

    /// <summary>
    /// Culture used for numeric formatting.
    /// Default: InvariantCulture.
    /// </summary>
    public CultureInfo Culture { get; init; } = CultureInfo.InvariantCulture;
}
