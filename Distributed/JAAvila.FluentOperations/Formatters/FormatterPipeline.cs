namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Central pipeline for value formatting. Dispatches to registered IValueFormatter implementations
/// based on type and priority.
/// </summary>
public static class FormatterPipeline
{
    private static readonly List<IValueFormatter> BuiltInFormatters = [];
    private static readonly object Lock = new();
    private static bool _initialized;

    /// <summary>
    /// Ensures all built-in formatters are registered. Called lazily on first use.
    /// </summary>
    private static void EnsureInitialized()
    {
        if (_initialized)
        {
            return;
        }

        lock (Lock)
        {
            if (_initialized)
            {
                return;
            }

            BuiltInFormatters.Add(new StringFormatter.StringValueFormatter());
            BuiltInFormatters.Add(new BooleanFormatter.BooleanValueFormatter());
            BuiltInFormatters.Add(new NumericFormatter());
            BuiltInFormatters.Add(new DateTimeFormatter());
            BuiltInFormatters.Add(new CollectionFormatter());
            BuiltInFormatters.Add(new EnumValueFormatter());
            BuiltInFormatters.Add(new TypeFormatter.TypeValueFormatter());

            BuiltInFormatters.Sort((a, b) => a.Priority.CompareTo(b.Priority));

            _initialized = true;
        }
    }

    /// <summary>
    /// Formats a value using the first matching formatter in priority order.
    /// Falls back to ToString() if no formatter matches.
    /// </summary>
    /// <param name="value">The value to format. May be null.</param>
    /// <param name="type">The declared type of the value.</param>
    /// <returns>Formatted string representation.</returns>
    public static string Format(object? value, Type type)
    {
        EnsureInitialized();
        var context = FormattingContext.FromCurrentConfig();
        return FormatWithContext(value, type, context);
    }

    /// <summary>
    /// Formats a value using the given context (for recursive formatting).
    /// </summary>
    internal static string FormatWithContext(object? value, Type type, FormattingContext context)
    {
        if (value is null)
        {
            return context.NullDisplay;
        }

        // Check Nullable<T> -- unwrap to the underlying type
        var actualType = Nullable.GetUnderlyingType(type) ?? type;

        EnsureInitialized();

        // Find the first formatter that can handle this type (the list is pre-sorted by priority)
        var formatter = BuiltInFormatters.FirstOrDefault(f => f.CanHandle(actualType));

        if (formatter is not null)
        {
            return formatter.Format(value, context);
        }

        // Fallback: ToString()
        return value.ToString() ?? context.NullDisplay;
    }

    /// <summary>
    /// Registers a custom formatter. Custom formatters are checked before built-in ones if they
    /// have lower priority values.
    /// </summary>
    /// <param name="formatter">The custom formatter to register.</param>
    public static void Register(IValueFormatter formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);
        EnsureInitialized();

        lock (Lock)
        {
            BuiltInFormatters.Add(formatter);
            BuiltInFormatters.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }
    }

    /// <summary>
    /// Registers a custom formatter using a lambda for a specific type.
    /// Priority defaults to 5 (higher than all built-in formatters).
    /// </summary>
    /// <typeparam name="T">The type to format.</typeparam>
    /// <param name="format">The formatting function.</param>
    public static void Register<T>(Func<T, string> format)
    {
        ArgumentNullException.ThrowIfNull(format);
        Register(new LambdaFormatter<T>(format));
    }

    /// <summary>
    /// Resets the pipeline to built-in formatters only. Useful in test teardown.
    /// </summary>
    public static void Reset()
    {
        lock (Lock)
        {
            BuiltInFormatters.Clear();
            _initialized = false;
        }
    }

    // --- Internal formatter implementations ---

    /// <summary>
    /// Formatter for enum types.
    /// </summary>
    private class EnumValueFormatter : IValueFormatter
    {
        public bool CanHandle(Type type) => type.IsEnum;

        public string Format(object? value, FormattingContext context)
        {
            return value?.ToString() ?? context.NullDisplay;
        }

        public int Priority => 60;
    }

    /// <summary>
    /// Generic lambda-based formatter.
    /// </summary>
    private class LambdaFormatter<T>(Func<T, string> format) : IValueFormatter
    {
        public bool CanHandle(Type type) => typeof(T).IsAssignableFrom(type);

        public string Format(object? value, FormattingContext context)
        {
            if (value is T typed)
            {
                return format(typed);
            }

            return value?.ToString() ?? context.NullDisplay;
        }

        public int Priority => 5;
    }
}
