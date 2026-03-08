namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Formatter for date/time types: DateTime, DateOnly, TimeOnly, TimeSpan.
/// Supports configurable format strings via DateTimeConfig.
/// </summary>
internal class DateTimeFormatter : IValueFormatter
{
    private static readonly HashSet<Type> DateTimeTypes =
    [
        typeof(DateTime),
        typeof(DateOnly),
        typeof(TimeOnly),
        typeof(TimeSpan),
        typeof(DateTimeOffset)
    ];

    public bool CanHandle(Type type) => DateTimeTypes.Contains(type);

    public string Format(object? value, FormattingContext context)
    {
        if (value is null)
        {
            return context.NullDisplay;
        }

        var config = context.DateTimeConfig;

        return value switch
        {
            DateTime dt when dt == DateTime.MinValue => "<DateTime.MinValue>",
            DateTime dt when dt == DateTime.MaxValue => "<DateTime.MaxValue>",
            DateTime dt => dt.ToString(config.Format),
            DateOnly d => d.ToString(config.DateOnlyFormat),
            TimeOnly t => t.ToString(config.TimeOnlyFormat),
            TimeSpan ts when config.TimeSpanFormat is not null
                => ts.ToString(config.TimeSpanFormat),
            TimeSpan ts => ts.ToString(),
            DateTimeOffset dto => dto.ToString(config.Format),
            _ => value.ToString()!
        };
    }

    public int Priority => 40;
}
