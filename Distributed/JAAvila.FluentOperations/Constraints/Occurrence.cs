namespace JAAvila.FluentOperations.Constraints;

/// <summary>
/// Entry point for building occurrence constraints used in collection assertions.
/// Provides factory properties and methods for all supported occurrence modes.
/// </summary>
public class Occurrence
{
    /// <summary>
    /// Creates an adapter that passes when the actual count equals the specified number.
    /// </summary>
    public static OccurrenceAdapter Exactly => new((o, a) => o == a, "exactly");

    /// <summary>
    /// Creates an adapter that passes when the actual count is greater than or equal to the specified number.
    /// </summary>
    public static OccurrenceAdapter AtLeast => new((o, a) => o <= a, "at least");

    /// <summary>
    /// Creates an adapter that passes when the actual count is less than or equal to the specified number.
    /// </summary>
    public static OccurrenceAdapter AtMost => new((o, a) => o >= a, "at most");

    /// <summary>
    /// Creates an adapter that passes when the actual count is strictly greater than the specified number.
    /// </summary>
    public static OccurrenceAdapter MoreThan => new((o, a) => o < a, "more than");

    /// <summary>
    /// Creates an adapter that passes when the actual count is strictly less than the specified number.
    /// </summary>
    public static OccurrenceAdapter LessThan => new((o, a) => o > a, "less than");

    /// <summary>
    /// Creates a range-based occurrence constraint (inclusive).
    /// Usage: Occurrence.Between(2, 5) -- matches if the count is 2, 3, 4, or 5.
    /// </summary>
    public static OccurrenceBetweenAdapter Between(int min, int max) => new(min, max);
}
