using System.Diagnostics.CodeAnalysis;

namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Represents a failure reason with an optional composite format message and substitution arguments.
/// Used to attach contextual failure messages to assertions and quality rules.
/// </summary>
/// <param name="Because">A composite-format string describing why the assertion is expected to fail.
/// Supports <see cref="string.Format(string,object[])"/> placeholders such as <c>{0}</c>.</param>
/// <param name="Arguments">Optional arguments substituted into the <paramref name="Because"/> format string.</param>
public record Fail(
    [StringSyntax("CompositeFormat")] string Because = "",
    params object[] Arguments)
{
    /// <summary>
    /// Creates a new <see cref="Fail"/> instance with the given format string and arguments.
    /// </summary>
    /// <param name="because">A composite-format string describing the failure reason.</param>
    /// <param name="arguments">Optional arguments substituted into <paramref name="because"/>.</param>
    /// <returns>A new <see cref="Fail"/> instance.</returns>
    public static Fail New(
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] arguments
    ) => new(because, arguments);
    
    public override string ToString()
    {
        return HasArguments ? string.Format(Because, Arguments) : Because;
    }

    #region PRIVATE METHODS

    private bool HasArguments => Arguments.Length > 0;

    #endregion
}