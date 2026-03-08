namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Interface for value formatters in the formatting pipeline.
/// Implementations provide type-specific formatting for error messages.
/// </summary>
public interface IValueFormatter
{
    /// <summary>
    /// Determines whether this formatter can handle the specified type.
    /// </summary>
    /// <param name="type">The runtime type to check.</param>
    /// <returns>true if this formatter can format values of the given type.</returns>
    bool CanHandle(Type type);

    /// <summary>
    /// Formats the given value into a display string for error messages.
    /// </summary>
    /// <param name="value">The value to format. Maybe null.</param>
    /// <param name="context">The formatting context with configuration options.</param>
    /// <returns>A formatted string representation of the value.</returns>
    string Format(object? value, FormattingContext context);

    /// <summary>
    /// Priority of this formatter. Lower values have higher priority.
    /// Built-in formatters use 10-70. Custom formatters should use lower values to override.
    /// </summary>
    int Priority { get; }
}
