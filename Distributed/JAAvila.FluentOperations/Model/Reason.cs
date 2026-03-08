using System.Diagnostics.CodeAnalysis;

namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Represents a reason with a descriptive text and optional arguments that can be used
/// in various operations to provide explanatory context.
/// </summary>
public record Reason(
    [StringSyntax("CompositeFormat")] string Because = "",
    params object[] Arguments
)
{
    /// <summary>
    /// Creates a new instance of the <see cref="Reason"/> record with a given descriptive text
    /// and optional arguments for explanatory context.
    /// </summary>
    /// <param name="because">The descriptive text providing the reason.</param>
    /// <param name="arguments">Optional arguments to include in the reason.</param>
    /// <returns>A new instance of the <see cref="Reason"/> record.</returns>
    public static Reason New(
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] arguments
    ) => new(because, arguments);

    /// <summary>
    /// Creates a string representation of the <see cref="Reason"/> record, optionally formatting the
    /// descriptive text using the provided arguments if they exist.
    /// </summary>
    /// <returns>A formatted string representation of the reason. If arguments are provided, the string
    /// will be formatted using the arguments; otherwise, the raw descriptive text is returned.</returns>
    public override string ToString()
    {
        return HasArguments ? string.Format(Because, Arguments) : Because;
    }

    #region PRIVATE METHODS

    private bool HasArguments => Arguments.Length > 0;

    #endregion
}
