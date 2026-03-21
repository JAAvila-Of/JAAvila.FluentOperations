using System.Collections.Concurrent;
using System.Globalization;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Immutable localization configuration. Holds the provider, culture, and optional message cache.
/// </summary>
internal class LocalizationConfig
{
    private readonly ConcurrentDictionary<string, string?> _cache = new(StringComparer.Ordinal);

    /// <summary>
    /// The message provider used to resolve localized messages.
    /// </summary>
    public IMessageProvider? Provider { get; init; }

    /// <summary>
    /// The culture used for message resolution.
    /// </summary>
    public CultureInfo Culture { get; init; } = CultureInfo.CurrentUICulture;

    /// <summary>
    /// When <c>true</c>, resolved messages are cached per key to avoid repeated provider calls.
    /// </summary>
    public bool EnableCache { get; init; } = true;

    /// <summary>
    /// Resolves the localized message for the given key, using the cache when enabled.
    /// Returns <c>null</c> if no provider is configured or the key is not found.
    /// </summary>
    internal string? ResolveMessage(string messageKey)
    {
        if (Provider is null) return null;

        if (!EnableCache)
        {
            return Provider.GetMessage(messageKey, Culture);
        }

        var cacheKey = $"{Culture.Name}:{messageKey}";
        return _cache.GetOrAdd(cacheKey, _ => Provider.GetMessage(messageKey, Culture));
    }
}
