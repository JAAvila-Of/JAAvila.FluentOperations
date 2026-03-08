using JAAvila.FluentOperations.Common;

namespace JAAvila.FluentOperations.Constraints;

/// <summary>
/// Intermediate adapter returned by <see cref="Occurrence"/> factory properties.
/// Combines an occurrence comparison delegate with a human-readable description,
/// and exposes convenience methods for producing final <see cref="OccurrenceConstraint"/> instances.
/// </summary>
/// <param name="comparer">The delegate that compares the expected and actual occurrence counts.</param>
/// <param name="description">A human-readable description of the comparison mode (e.g., "at least").</param>
public class OccurrenceAdapter(OccurrenceDelegate comparer, string description)
{
    /// <summary>
    /// Builds an <see cref="OccurrenceConstraint"/> that matches exactly one occurrence.
    /// </summary>
    /// <returns>An <see cref="OccurrenceConstraint"/> for one occurrence.</returns>
    public OccurrenceConstraint Once() => new(1, comparer, description, "once");

    /// <summary>
    /// Builds an <see cref="OccurrenceConstraint"/> that matches exactly two occurrences.
    /// </summary>
    /// <returns>An <see cref="OccurrenceConstraint"/> for two occurrences.</returns>
    public OccurrenceConstraint Twice() => new(2, comparer, description, "twice");

    /// <summary>
    /// Builds an <see cref="OccurrenceConstraint"/> that matches exactly three occurrences.
    /// </summary>
    /// <returns>An <see cref="OccurrenceConstraint"/> for three occurrences.</returns>
    public OccurrenceConstraint Thrice() => new(3, comparer, description, "thrice");

    /// <summary>
    /// Builds an <see cref="OccurrenceConstraint"/> for an arbitrary number of occurrences.
    /// </summary>
    /// <param name="times">The expected number of occurrences.</param>
    /// <returns>An <see cref="OccurrenceConstraint"/> for the specified number of occurrences.</returns>
    public OccurrenceConstraint Times(int times) =>
        new(times, comparer, description, $"{(times == 1 ? "1 time" : $"{times} times")}");
}
