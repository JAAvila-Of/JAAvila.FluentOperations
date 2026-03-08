namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Generic value formatter used by managers to format values in error messages.
/// Delegates to FormatterPipeline for type-specific formatting.
/// </summary>
internal class BaseFormatter
{
    /// <summary>
    /// Formats a value of the given type for display in error messages.
    /// Uses the FormatterPipeline to dispatch to type-specific formatters.
    /// </summary>
    /// <typeparam name="TType">The declared type of the value.</typeparam>
    /// <param name="value">The value to format.</param>
    /// <returns>A formatted string representation.</returns>
    public static string Format<TType>(TType value)
    {
        return FormatterPipeline.Format(value, typeof(TType));
    }
}
