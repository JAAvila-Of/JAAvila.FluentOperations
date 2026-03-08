using JAAvila.FluentOperations.Common;

namespace JAAvila.FluentOperations.Constraints;

/// <summary>
/// Represents a resolved occurrence constraint ready for evaluation against an actual count.
/// Produced by <see cref="OccurrenceAdapter"/> methods such as <c>Once()</c>, <c>Twice()</c>, and <c>Times(n)</c>.
/// </summary>
/// <param name="occurrence">The reference occurrence count used by the comparison delegate.</param>
/// <param name="comparer">The delegate that compares the expected and actual counts.</param>
/// <param name="description">A human-readable description of the comparison mode (e.g., "at least").</param>
/// <param name="timesString">A human-readable representation of the occurrence count (e.g., "once", "3 times").</param>
public class OccurrenceConstraint(
    int occurrence,
    OccurrenceDelegate comparer,
    string description,
    string timesString
)
{
    /// <summary>
    /// Evaluates the constraint against the actual observed count.
    /// </summary>
    /// <param name="actual">The actual number of times the element was found in the collection.</param>
    /// <returns><c>true</c> if the constraint is satisfied; otherwise <c>false</c>.</returns>
    public bool Validate(int actual)
    {
        return comparer(occurrence, actual);
    }

    /// <summary>
    /// Returns a human-readable representation of this constraint, e.g. "at least twice".
    /// </summary>
    public override string ToString() => $"{description} {timesString}";
}
