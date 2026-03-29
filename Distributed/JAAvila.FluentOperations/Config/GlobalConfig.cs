using System.Reflection;
using JAAvila.FluentOperations.Utils;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Internal configuration handler. Uses AsyncLocal for thread-safe per-flow configuration.
/// Public API is FluentOperationsConfig; this class is the internal engine.
/// </summary>
internal class GlobalConfig
{
    private static readonly AsyncLocal<GlobalConfig?> Instance = new();

    /// <summary>
    /// Static cache for the default config resolved from [FluentOperationsOptions] attribute
    /// scanning. Computed once using StackTrace on the first Initialize() call, then reused for
    /// all later calls — eliminating repeated StackTrace allocations.
    /// </summary>
    private static readonly Lazy<GlobalConfig> CachedDefault = new(
        ResolveDefault,
        LazyThreadSafetyMode.ExecutionAndPublication
    );

    private StringConfig String { get; init; } = new();
    private NumericConfig Numeric { get; init; } = new();
    private DateTimeConfig DateTime { get; init; } = new();
    private FormattingConfig Formatting { get; init; } = new();
    private TestFrameworkConfig TestFramework { get; init; } = new();
    private LocalizationConfig? Localization { get; init; }
    private TelemetryConfig? Telemetry { get; init; }

    private static GlobalConfig Default => new()
    {
        String = new StringConfig { MaxDisplayLength = 30 },
        Numeric = new NumericConfig(),
        DateTime = new DateTimeConfig(),
        Formatting = new FormattingConfig(),
        TestFramework = new TestFrameworkConfig()
    };

    /// <summary>
    /// Resolves the default GlobalConfig by scanning the call stack for a type decorated with
    /// [FluentOperationsOptions]. This runs ONCE (via Lazy) and the result is cached statically.
    /// </summary>
    private static GlobalConfig ResolveDefault()
    {
        var types = StackUtils.GetTypesFromStack();
        var t = types.FirstOrDefault(
            x => x.GetCustomAttribute<FluentOperationsOptionsAttribute>() is not null
        );

        if (t is null)
        {
            return Default;
        }

        var a = t.GetCustomAttribute<FluentOperationsOptionsAttribute>()!;
        return BuildFromAttribute(a);
    }

    public static void Initialize()
    {
        if (Instance.Value is not null)
        {
            return;
        }

        // Use the statically cached default (resolves StackTrace only once ever)
        Instance.Value = CachedDefault.Value;
    }

    private static GlobalConfig GetInstance()
    {
        if (Instance.Value is null)
        {
            Initialize();
        }

        return Instance.Value!;
    }

    private static void SetInstance(GlobalConfig value) => Instance.Value = value;

    private static GlobalConfig BuildFromAttribute(FluentOperationsOptionsAttribute attribute)
    {
        return new GlobalConfig
        {
            String = new StringConfig
            {
                MaxDisplayLength = attribute.Conditional(
                    x => x.StringMaxDisplayLength >= 10,
                    x => x.StringMaxDisplayLength,
                    _ => 10
                )
            },
            Numeric = attribute.NumericDecimalPlaces >= 0
                ? new NumericConfig { DecimalPlaces = attribute.NumericDecimalPlaces }
                : new NumericConfig(),
            DateTime = attribute.DateTimeFormat is not null
                ? new DateTimeConfig { Format = attribute.DateTimeFormat }
                : new DateTimeConfig(),
            Formatting = attribute.NullDisplay is not null
                ? new FormattingConfig { NullDisplay = attribute.NullDisplay }
                : new FormattingConfig()
        };
    }

    // --- Public static accessors (used by formatters and managers) ---

    public static StringConfig GetStringConfig() => GetInstance().String;
    public static NumericConfig GetNumericConfig() => GetInstance().Numeric;
    public static DateTimeConfig GetDateTimeConfig() => GetInstance().DateTime;
    public static FormattingConfig GetFormattingConfig() => GetInstance().Formatting;
    public static TestFrameworkConfig GetTestFrameworkConfig() => GetInstance().TestFramework;
    public static LocalizationConfig? GetLocalizationConfig() => GetInstance().Localization;
    public static TelemetryConfig? GetTelemetryConfig() => GetInstance().Telemetry;

    // --- Internal accessors for ConfigBuilder.FromExisting() ---

    internal StringConfig GetStringConfigInternal() => String;
    internal NumericConfig GetNumericConfigInternal() => Numeric;
    internal DateTimeConfig GetDateTimeConfigInternal() => DateTime;
    internal FormattingConfig GetFormattingConfigInternal() => Formatting;
    internal TestFrameworkConfig GetTestFrameworkConfigInternal() => TestFramework;
    internal LocalizationConfig? GetLocalizationConfigInternal() => Localization;
    internal TelemetryConfig? GetTelemetryConfigInternal() => Telemetry;

    // --- Scope management (used by FluentOperationsConfig) ---

    internal static GlobalConfig GetCurrentConfig()
    {
        return GetInstance();
    }

    internal static void ApplyConfig(ConfigBuilder builder)
    {
        var previousFramework = Instance.Value?.TestFramework.Framework;
        var newTestFrameworkConfig = builder.BuildTestFrameworkConfig();

        Instance.Value = new GlobalConfig
        {
            String = builder.BuildStringConfig(),
            Numeric = builder.BuildNumericConfig(),
            DateTime = builder.BuildDateTimeConfig(),
            Formatting = builder.BuildFormattingConfig(),
            TestFramework = newTestFrameworkConfig,
            Localization = builder.BuildLocalizationConfig(),
            Telemetry = builder.BuildTelemetryConfig()
        };

        if (previousFramework != newTestFrameworkConfig.Framework)
        {
            Handler.ExceptionHandler.InvalidateCache();
        }
    }

    internal static void RestoreConfig(GlobalConfig previous)
    {
        var currentFramework = Instance.Value?.TestFramework.Framework;
        Instance.Value = previous;

        if (currentFramework != previous.TestFramework.Framework)
        {
            Handler.ExceptionHandler.InvalidateCache();
        }
    }

    internal static void ResetConfig()
    {
        Instance.Value = null;
        Handler.ExceptionHandler.InvalidateCache();
    }
}
