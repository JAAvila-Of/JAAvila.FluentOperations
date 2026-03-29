using System.Globalization;
using System.Resources;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Localization;

/// <summary>
/// An <see cref="IMessageProvider"/> backed by a <see cref="ResourceManager"/>.
/// Resource keys are derived from message keys by replacing dots with underscores
/// because .resx resource names cannot contain dots.
/// </summary>
/// <remarks>
/// For example, a message key of <c>"String.BeEmail"</c> maps to resource name <c>"String_BeEmail"</c>.
/// </remarks>
public class ResourceManagerProvider(ResourceManager resourceManager) : IMessageProvider
{
    /// <inheritdoc />
    public string? GetMessage(string messageKey, CultureInfo culture)
    {
        // .resx resource names cannot contain dots, so replace it with underscores.
        var resourceName = messageKey.Replace('.', '_');
        return resourceManager.GetString(resourceName, culture);
    }
}
