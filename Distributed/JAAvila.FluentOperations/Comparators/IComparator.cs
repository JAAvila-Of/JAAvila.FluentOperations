namespace JAAvila.FluentOperations.Comparators;

/// <summary>
/// Generic interface for value comparators.
/// Implementations provide type-specific comparison logic with optional configuration.
/// </summary>
/// <typeparam name="T">The type of values to compare.</typeparam>
public interface IComparator<in T>
{
    /// <summary>
    /// Compares two values using default comparison options.
    /// </summary>
    ComparisonResult Compare(T actual, T expected);

    /// <summary>
    /// Compares two values using the specified comparison options.
    /// </summary>
    ComparisonResult Compare(T actual, T expected, ComparisonOptions options)
        => Compare(actual, expected); // default implementation for backward compat
}
