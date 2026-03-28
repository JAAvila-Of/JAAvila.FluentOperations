using System.ComponentModel.DataAnnotations;

namespace JAAvila.FluentOperations;

/// <summary>
/// Specifies configurable options for a class to customize behavior in the context of FluentOperations.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class FluentOperationsOptionsAttribute([MinLength(10)] int stringMaxDisplayLength)
    : Attribute
{
    /// <summary>
    /// Maximum length for string values to be displayed.
    /// </summary>
    public int StringMaxDisplayLength { get; } = stringMaxDisplayLength;

    /// <summary>
    /// Number of decimal places for numeric display. -1 means not configured (use default).
    /// </summary>
    public int NumericDecimalPlaces { get; set; } = -1;

    /// <summary>
    /// Format string for DateTime display. Null means not configured (use default).
    /// </summary>
    public string? DateTimeFormat { get; set; }

    /// <summary>
    /// Display text for null values. null means not configured (use default).
    /// </summary>
    public string? NullDisplay { get; set; }
}
