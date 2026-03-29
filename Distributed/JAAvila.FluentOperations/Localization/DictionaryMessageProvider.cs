using System.Globalization;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Localization;

/// <summary>
/// An <see cref="IMessageProvider"/> backed by an in-memory dictionary.
/// Useful for testing and for simple fixed-locale scenarios.
/// </summary>
/// <remarks>
/// Keys are <c>(messageKey, cultureName)</c> pairs. Use an empty culture name
/// (<c>""</c>) to provide a culture-neutral fallback entry.
/// </remarks>
public class DictionaryMessageProvider : IMessageProvider
{
    private readonly Dictionary<(string key, string culture), string> _messages;

    /// <summary>
    /// Provides localized messages backed by an in-memory dictionary implementation.
    /// </summary>
    /// <remarks>
    /// The dictionary keys are tuples of message keys and culture names. A culture-neutral fallback
    /// can be provided by using an empty string as the culture name.
    /// </remarks>
    public DictionaryMessageProvider(Dictionary<(string key, string culture), string> messages)
    {
        ArgumentNullException.ThrowIfNull(messages);
        _messages = messages;
    }

    /// <inheritdoc />
    public string? GetMessage(string messageKey, CultureInfo culture)
    {
        // Try an exact culture match first, then neutral culture (empty string).
        return _messages.TryGetValue((messageKey, culture.Name), out var message)
            ? message
            : _messages.GetValueOrDefault((messageKey, ""));
    }
}
