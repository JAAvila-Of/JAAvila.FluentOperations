namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Formats boolean values for display in failure messages, rendering them as the literals
/// <c>true</c> or <c>false</c> and labelling null nullable booleans as <c>&lt;null&gt;</c>.
/// Also exposes <see cref="BooleanValueFormatter"/>, the <see cref="IValueFormatter"/> adapter
/// used by the <see cref="FormatterPipeline"/>.
/// </summary>
internal class BooleanFormatter
{
    public static string Format(bool? value)
    {
        if (value is null)
        {
            return "<null>";
        }

        return value.Value ? "true" : "false";
    }

    /// <summary>
    /// IValueFormatter adapter for the BooleanFormatter.
    /// Used by the FormatterPipeline.
    /// </summary>
    internal class BooleanValueFormatter : IValueFormatter
    {
        public bool CanHandle(Type type) => type == typeof(bool);

        public string Format(object? value, FormattingContext context)
        {
            if (value is null)
                return context.NullDisplay;

            return (bool)value ? "true" : "false";
        }

        public int Priority => 20;
    }
}
