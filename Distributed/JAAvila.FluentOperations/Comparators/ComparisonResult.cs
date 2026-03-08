namespace JAAvila.FluentOperations.Comparators;

/// <summary>
/// Represents the result of a comparison operation.
/// </summary>
/// <param name="AreEqual">Whether the two values are considered equal.</param>
/// <param name="DifferenceDescription">Optional description of the difference, if any.</param>
public record ComparisonResult(bool AreEqual, string? DifferenceDescription = null)
{
    /// <summary>
    /// A shared instance representing a successful (equal) comparison.
    /// </summary>
    public static readonly ComparisonResult Equal = new(true);

    /// <summary>
    /// Creates a failed comparison result with a difference description.
    /// </summary>
    public static ComparisonResult NotEqual(string differenceDescription)
        => new(false, differenceDescription);
}
