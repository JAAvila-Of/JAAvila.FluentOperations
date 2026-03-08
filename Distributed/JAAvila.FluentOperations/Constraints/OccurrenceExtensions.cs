namespace JAAvila.FluentOperations.Constraints;

/// <summary>
/// Extension methods on int for fluent occurrence constraints.
/// Allows syntax like: 2.TimesExactly(), 3.TimesOrMore(), 1.TimesOrLess()
/// </summary>
public static class OccurrenceExtensions
{
    /// <summary>
    /// Creates an occurrence constraint requiring exactly this many occurrences.
    /// Equivalent to Occurrence.Exactly.Times(n).
    /// </summary>
    public static OccurrenceConstraint TimesExactly(this int times)
        => Occurrence.Exactly.Times(times);

    /// <summary>
    /// Creates an occurrence constraint requiring at least this many occurrences.
    /// Equivalent to Occurrence.AtLeast.Times(n).
    /// </summary>
    public static OccurrenceConstraint TimesOrMore(this int times)
        => Occurrence.AtLeast.Times(times);

    /// <summary>
    /// Creates an occurrence constraint requiring at most this many occurrences.
    /// Equivalent to Occurrence.AtMost.Times(n).
    /// </summary>
    public static OccurrenceConstraint TimesOrLess(this int times)
        => Occurrence.AtMost.Times(times);
}
