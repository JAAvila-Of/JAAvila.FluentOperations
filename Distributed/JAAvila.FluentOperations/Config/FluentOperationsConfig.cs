using System.Globalization;

namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Public API for centralized configuration of the FluentOperations framework.
/// Provides programmatic configuration, scoped configuration, and access to all config sections.
/// </summary>
/// <remarks>
/// Usage:
/// <code>
/// // Programmatic configuration (call once, typically in test setup or app startup)
/// FluentOperationsConfig.Configure(config =>
/// {
///     config.String.MaxDisplayLength = 50;
///     config.Numeric.DecimalPlaces = 4;
///     config.Formatting.NullDisplay = "&lt;nulo&gt;";
/// });
///
/// // Scoped configuration (temporary override)
/// using (FluentOperationsConfig.Scope(config =>
/// {
///     config.String.MaxDisplayLength = 100;
/// }))
/// {
///     longString.Test().NotBeEmpty(); // uses MaxDisplayLength = 100
/// }
/// // here it reverts to previous config
/// </code>
/// </remarks>
public static class FluentOperationsConfig
{
    /// <summary>
    /// Configures the global FluentOperations settings programmatically.
    /// This method initializes GlobalConfig if not already done, then applies the configuration action.
    /// </summary>
    /// <param name="configure">Action to configure the settings. Receives a mutable ConfigBuilder.</param>
    public static void Configure(Action<ConfigBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        var builder = new ConfigBuilder();
        configure(builder);
        GlobalConfig.ApplyConfig(builder);
    }

    /// <summary>
    /// Creates a temporary configuration scope. The previous configuration is restored when the scope is disposed.
    /// Thread-safe via AsyncLocal: each async flow has its own config stack.
    /// </summary>
    /// <param name="configure">Action to configure the scoped settings.</param>
    /// <returns>An IDisposable that restores the previous configuration when disposed.</returns>
    public static ConfigScope Scope(Action<ConfigBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        var previous = GlobalConfig.GetCurrentConfig();
        var builder = ConfigBuilder.FromExisting(previous);
        configure(builder);
        GlobalConfig.ApplyConfig(builder);
        return new ConfigScope(previous);
    }

    /// <summary>
    /// Resets the configuration to defaults. Useful in test teardown.
    /// </summary>
    public static void Reset()
    {
        GlobalConfig.ResetConfig();
    }

    /// <summary>
    /// Returns the current effective StringConfig.
    /// </summary>
    internal static StringConfig GetStringConfig() => GlobalConfig.GetStringConfig();

    /// <summary>
    /// Returns the current effective NumericConfig.
    /// </summary>
    internal static NumericConfig GetNumericConfig() => GlobalConfig.GetNumericConfig();

    /// <summary>
    /// Returns the current effective DateTimeConfig.
    /// </summary>
    internal static DateTimeConfig GetDateTimeConfig() => GlobalConfig.GetDateTimeConfig();

    /// <summary>
    /// Returns the current effective FormattingConfig.
    /// </summary>
    internal static FormattingConfig GetFormattingConfig() => GlobalConfig.GetFormattingConfig();
}

/// <summary>
/// Mutable builder used during FluentOperationsConfig.Configure() and .Scope().
/// Not exposed outside configuration lambdas.
/// </summary>
public class ConfigBuilder
{
    /// <summary>
    /// Configuration options for string display and truncation behaviour.
    /// </summary>
    public StringConfigBuilder String { get; } = new();

    /// <summary>
    /// Configuration options for numeric formatting such as decimal places and thousands separators.
    /// </summary>
    public NumericConfigBuilder Numeric { get; } = new();

    /// <summary>
    /// Configuration options for date and time display formats.
    /// </summary>
    public DateTimeConfigBuilder DateTime { get; } = new();

    /// <summary>
    /// Configuration options for general output formatting such as null display text and collection limits.
    /// </summary>
    public FormattingConfigBuilder Formatting { get; } = new();

    internal static ConfigBuilder FromExisting(GlobalConfig existing)
    {
        var builder = new ConfigBuilder();
        var sc = existing.GetStringConfigInternal();
        builder.String.MaxDisplayLength = sc.MaxDisplayLength;

        var nc = existing.GetNumericConfigInternal();
        builder.Numeric.DecimalPlaces = nc.DecimalPlaces;
        builder.Numeric.UseThousandsSeparator = nc.UseThousandsSeparator;
        builder.Numeric.Culture = nc.Culture;

        var dc = existing.GetDateTimeConfigInternal();
        builder.DateTime.Format = dc.Format;
        builder.DateTime.DateOnlyFormat = dc.DateOnlyFormat;
        builder.DateTime.TimeOnlyFormat = dc.TimeOnlyFormat;
        builder.DateTime.TimeSpanFormat = dc.TimeSpanFormat;

        var fc = existing.GetFormattingConfigInternal();
        builder.Formatting.NullDisplay = fc.NullDisplay;
        builder.Formatting.EmptyDisplay = fc.EmptyDisplay;
        builder.Formatting.MaxCollectionItems = fc.MaxCollectionItems;
        builder.Formatting.MaxDepth = fc.MaxDepth;

        return builder;
    }

    internal StringConfig BuildStringConfig() =>
        new() { MaxDisplayLength = Math.Max(String.MaxDisplayLength, 10) };

    internal NumericConfig BuildNumericConfig() =>
        new()
        {
            DecimalPlaces = Numeric.DecimalPlaces,
            UseThousandsSeparator = Numeric.UseThousandsSeparator,
            Culture = Numeric.Culture
        };

    internal DateTimeConfig BuildDateTimeConfig() =>
        new()
        {
            Format = DateTime.Format,
            DateOnlyFormat = DateTime.DateOnlyFormat,
            TimeOnlyFormat = DateTime.TimeOnlyFormat,
            TimeSpanFormat = DateTime.TimeSpanFormat
        };

    internal FormattingConfig BuildFormattingConfig() =>
        new()
        {
            NullDisplay = Formatting.NullDisplay,
            EmptyDisplay = Formatting.EmptyDisplay,
            MaxCollectionItems = Math.Max(Formatting.MaxCollectionItems, 1),
            MaxDepth = Math.Max(Formatting.MaxDepth, 1)
        };
}

/// <summary>
/// Mutable builder for StringConfig.
/// </summary>
public class StringConfigBuilder
{
    /// <summary>
    /// Maximum number of characters displayed when a string value appears in a failure message.
    /// Longer strings are truncated. Minimum effective value is 10.
    /// </summary>
    public int MaxDisplayLength { get; set; } = 30;
}

/// <summary>
/// Mutable builder for NumericConfig.
/// </summary>
public class NumericConfigBuilder
{
    /// <summary>
    /// Number of decimal places shown for floating-point values in failure messages.
    /// Use <c>-1</c> to preserve the natural precision of the value.
    /// </summary>
    public int DecimalPlaces { get; set; } = -1;

    /// <summary>
    /// When <c>true</c>, numeric values in failure messages include a thousands separator
    /// according to the configured <see cref="Culture"/>.
    /// </summary>
    public bool UseThousandsSeparator { get; set; }

    /// <summary>
    /// The culture used for numeric formatting in failure messages.
    /// Defaults to <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
}

/// <summary>
/// Mutable builder for DateTimeConfig.
/// </summary>
public class DateTimeConfigBuilder
{
    /// <summary>
    /// Format string used when displaying <see cref="System.DateTime"/> and <see cref="System.DateTimeOffset"/> values
    /// in failure messages. Defaults to <c>"yyyy-MM-dd HH:mm:ss"</c>.
    /// </summary>
    public string Format { get; set; } = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// Format string used when displaying <see cref="System.DateOnly"/> values in failure messages.
    /// Defaults to <c>"yyyy-MM-dd"</c>.
    /// </summary>
    public string DateOnlyFormat { get; set; } = "yyyy-MM-dd";

    /// <summary>
    /// Format string used when displaying <see cref="System.TimeOnly"/> values in failure messages.
    /// Defaults to <c>"HH:mm:ss"</c>.
    /// </summary>
    public string TimeOnlyFormat { get; set; } = "HH:mm:ss";

    /// <summary>
    /// Optional custom format string for <see cref="System.TimeSpan"/> values.
    /// When <c>null</c>, the default <c>TimeSpan.ToString()</c> output is used.
    /// </summary>
    public string? TimeSpanFormat { get; set; }
}

/// <summary>
/// Mutable builder for FormattingConfig.
/// </summary>
public class FormattingConfigBuilder
{
    /// <summary>
    /// Text used to represent a <c>null</c> value in failure messages. Defaults to <c>"&lt;null&gt;"</c>.
    /// </summary>
    public string NullDisplay { get; set; } = "<null>";

    /// <summary>
    /// Text used to represent an empty string in failure messages. Defaults to <c>"\"\" (&lt;empty&gt;)"</c>.
    /// </summary>
    public string EmptyDisplay { get; set; } = "\"\" (<empty>)";

    /// <summary>
    /// Maximum number of collection items shown inline in failure messages.
    /// Minimum effective value is 1. Defaults to <c>10</c>.
    /// </summary>
    public int MaxCollectionItems { get; set; } = 10;

    /// <summary>
    /// Maximum object graph depth explored when formatting nested objects.
    /// Minimum effective value is 1. Defaults to <c>3</c>.
    /// </summary>
    public int MaxDepth { get; set; } = 3;
}
