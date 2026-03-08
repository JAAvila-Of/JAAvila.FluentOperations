using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;

namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Formats string values for display in failure messages, truncating long strings symmetrically
/// and labelling null and empty values with human-readable placeholders.
/// Also exposes <see cref="StringValueFormatter"/>, the <see cref="IValueFormatter"/> adapter
/// used by the <see cref="FormatterPipeline"/>.
/// </summary>
internal class StringFormatter
{
    public static string Format(string? value)
    {
        if (value is null)
        {
            return "<null>";
        }

        if (value.Length == 0)
        {
            return "\"\" (<empty>)";
        }

        var stringConfig = GlobalConfig.GetStringConfig();

        if (value.Length <= stringConfig.MaxDisplayLength)
        {
            return $"\"{value}\" (length {value.Length})";
        }

        var middle = Convert.ToInt32(Math.Floor(stringConfig.MaxDisplayLength / 2D));

        return $"\"{value[..middle]}{Unicodes.Ellipsis}{value[^middle..]}\" (length {value.Length})";
    }

    /// <summary>
    /// IValueFormatter adapter for the StringFormatter.
    /// Used by the FormatterPipeline.
    /// </summary>
    internal class StringValueFormatter : IValueFormatter
    {
        public bool CanHandle(Type type) => type == typeof(string);

        public string Format(object? value, FormattingContext context)
        {
            if (value is null)
            {
                return context.NullDisplay;
            }

            var str = (string)value;

            if (str.Length == 0)
            {
                return context.EmptyDisplay;
            }

            if (str.Length <= context.MaxDisplayLength)
            {
                return $"\"{str}\" (length {str.Length})";
            }

            var middle = Convert.ToInt32(Math.Floor(context.MaxDisplayLength / 2D));
            return $"\"{str[..middle]}{Unicodes.Ellipsis}{str[^middle..]}\" (length {str.Length})";
        }

        public int Priority => 10;
    }
}
