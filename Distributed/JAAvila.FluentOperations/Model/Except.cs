using System.Diagnostics.CodeAnalysis;

namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Represents an exception-related model designed for creating formatted messages
/// or exceptions using a specific format string and optional arguments.
/// </summary>
public record Except(
    [StringSyntax("CompositeFormat")] string Because = "",
    params object[] Arguments
)
{
    /// <summary>
    /// Creates a new instance of the <see cref="Except"/> record with the provided message and arguments.
    /// </summary>
    /// <param name="because">The format string representing the cause or the message to describe the exception.</param>
    /// <param name="arguments">The optional array of objects to format within the message.</param>
    /// <returns>A new instance of the <see cref="Except"/> record populated with the specified message and arguments.</returns>
    public static Except New(
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] arguments
    ) => new(because, arguments);

    /// <summary>
    /// Returns a string representation of the <see cref="Except"/> record based on the provided format string and arguments.
    /// </summary>
    /// <returns>The formatted string when arguments are present, or the original format string if no arguments are provided.</returns>
    public override string ToString()
    {
        return HasArguments ? string.Format(Because, Arguments) : Because;
    }

    #region PRIVATE METHODS

    private bool HasArguments => Arguments.Length > 0;

    #endregion
}
