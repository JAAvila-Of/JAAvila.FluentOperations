using System.Globalization;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Mutable builder for <see cref="LocalizationConfig"/>. Used inside <see cref="FluentOperationsConfig.Configure"/>.
/// </summary>
public class LocalizationConfigBuilder
{
    /// <summary>
    /// The provider that supplies localized messages.
    /// </summary>
    public IMessageProvider? Provider { get; set; }

    /// <summary>
    /// The culture used for message resolution. Defaults to <see cref="CultureInfo.CurrentUICulture"/>.
    /// </summary>
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentUICulture;

    /// <summary>
    /// When <c>true</c>, resolved messages are cached per key. Default: <c>true</c>.
    /// </summary>
    public bool EnableCache { get; set; } = true;
}
