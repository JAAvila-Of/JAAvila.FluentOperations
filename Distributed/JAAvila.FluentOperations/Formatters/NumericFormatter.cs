using System.Globalization;
using JAAvila.FluentOperations.Config;

namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Formatter for numeric types: int, long, decimal, double, float, and their nullable counterparts.
/// Supports configurable decimal places, thousands of separators, and culture.
/// </summary>
internal class NumericFormatter : IValueFormatter
{
    private static readonly HashSet<Type> NumericTypes =
    [
        typeof(int),
        typeof(long),
        typeof(decimal),
        typeof(double),
        typeof(float),
        typeof(short),
        typeof(byte),
        typeof(sbyte),
        typeof(ushort),
        typeof(uint),
        typeof(ulong)
    ];

    public bool CanHandle(Type type) => NumericTypes.Contains(type);

    public string Format(object? value, FormattingContext context)
    {
        if (value is null)
        {
            return context.NullDisplay;
        }

        // Handle special floating-point values
        if (value is double d)
        {
            if (double.IsNaN(d))
            {
                return "<NaN>";
            }

            if (double.IsPositiveInfinity(d))
            {
                return "<+Infinity>";
            }

            if (double.IsNegativeInfinity(d))
            {
                return "<-Infinity>";
            }
        }

        if (value is float f)
        {
            if (float.IsNaN(f))
            {
                return "<NaN>";
            }

            if (float.IsPositiveInfinity(f))
            {
                return "<+Infinity>";
            }

            if (float.IsNegativeInfinity(f))
            {
                return "<-Infinity>";
            }
        }

        var config = context.NumericConfig;

        // If no custom formatting is configured, use default ToString() for backward compatibility
        if (config is { DecimalPlaces: < 0, UseThousandsSeparator: false })
        {
            return value.ToString()!;
        }

        // Build format string
        var formatString = BuildFormatString(config);
        return FormatWithString(value, formatString, config.Culture);
    }

    public int Priority => 30;

    private static string BuildFormatString(NumericConfig config)
    {
        if (config is { UseThousandsSeparator: true, DecimalPlaces: >= 0 })
        {
            return $"N{config.DecimalPlaces}";
        }

        if (config.UseThousandsSeparator)
        {
            return "N";
        }

        if (config.DecimalPlaces >= 0)
        {
            return $"F{config.DecimalPlaces}";
        }

        return "G"; // general format (fallback)
    }

    private static string FormatWithString(object value, string formatString, CultureInfo culture)
    {
        return value switch
        {
            int i => i.ToString(formatString, culture),
            long l => l.ToString(formatString, culture),
            decimal m => m.ToString(formatString, culture),
            double d => d.ToString(formatString, culture),
            float f => f.ToString(formatString, culture),
            short s => s.ToString(formatString, culture),
            byte b => b.ToString(formatString, culture),
            _ => value.ToString()!
        };
    }
}
