using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Formats <see cref="System.Type"/> values for display in failure messages, rendering them as
/// angle-bracketed friendly type names (e.g., <c>&lt;List&lt;int&gt;&gt;</c>).
/// Also exposes <see cref="TypeValueFormatter"/>, the <see cref="IValueFormatter"/> adapter
/// used by the <see cref="FormatterPipeline"/>.
/// </summary>
internal class TypeFormatter
{
    public static string FriendlyName(Type type)
    {
        return type.IsNull() ? "<NULL>" : $"<{type.FriendlyName()}>";
    }

    /// <summary>
    /// IValueFormatter adapter for Type values.
    /// Used by the FormatterPipeline.
    /// </summary>
    internal class TypeValueFormatter : IValueFormatter
    {
        public bool CanHandle(Type type) =>
            type == typeof(Type) || typeof(Type).IsAssignableFrom(type);

        public string Format(object? value, FormattingContext context)
        {
            return value is null ? "<NULL>" : FriendlyName((Type)value);
        }

        public int Priority => 70;
    }
}
