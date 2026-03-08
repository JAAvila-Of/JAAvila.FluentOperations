namespace JAAvila.FluentOperations.Constraints;

/// <summary>
/// Adapter that produces an OccurrenceConstraint for a range (between min and max, inclusive).
/// </summary>
public class OccurrenceBetweenAdapter(int min, int max)
{
    /// <summary>
    /// Creates the OccurrenceConstraint representing "between min and max times" (inclusive).
    /// </summary>
    private OccurrenceConstraint Build() =>
        new(
            0, // occurrence is not used; the delegate captures min and max directly
            (_, actual) => actual >= min && actual <= max,
            $"between {min} and {max}",
            "times"
        );

    /// <summary>
    /// Implicit conversion to OccurrenceConstraint for seamless API usage.
    /// </summary>
    public static implicit operator OccurrenceConstraint(OccurrenceBetweenAdapter adapter) =>
        adapter.Build();
}
