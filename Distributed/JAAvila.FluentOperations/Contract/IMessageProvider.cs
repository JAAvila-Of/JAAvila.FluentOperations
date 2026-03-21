using System.Globalization;

namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Defines a provider that resolves localized validation failure messages by key and culture.
/// </summary>
public interface IMessageProvider
{
    /// <summary>
    /// Returns the localized message for the given key and culture, or <c>null</c> if not found.
    /// </summary>
    /// <param name="messageKey">The message key (e.g., "String.BeEmail").</param>
    /// <param name="culture">The culture to use for resolution.</param>
    /// <returns>The localized message string, or <c>null</c> if no message is found for the key.</returns>
    string? GetMessage(string messageKey, CultureInfo culture);
}
