using JAAvila.FluentOperations.Common;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Utils;

/// <summary>
/// Provides utility methods for determining the type of given subject
/// based on its characteristics, such as whether it represents a boolean,
/// number, string literal, variable, function, or expression.
/// </summary>
internal static class SubjectUtils
{
    private static readonly string[] Operators =
    [
        "+",
        "-",
        "*",
        "/",
        "%",
        "&&",
        "||",
        "^",
        "?",
        ":"
    ];

    /// <summary>
    /// Determines the <see cref="SubjectType"/> of a given subject based on its characteristics.
    /// The subject could represent a boolean value, number, string literal, variable, function, or expression.
    /// </summary>
    /// <param name="subject">
    /// The string representation of the subject to analyze.
    /// </param>
    /// <returns>
    /// A <see cref="SubjectType"/> value that classifies the given subject into one of the categories
    /// (Void, Primitive, Variable, Function, or Expression).
    /// </returns>
    public static SubjectType GetSubjectType(string subject)
    {
        if (subject.IsNullOrEmpty())
        {
            return SubjectType.Void;
        }

        if (IsBoolean(subject) || IsNumber(subject) || IsStringLiteral(subject))
        {
            return SubjectType.Primitive;
        }

        if (IsValidVariableOrStaticProperty(subject))
        {
            return SubjectType.Variable;
        }

        return IsFunction(subject) ? SubjectType.Function : SubjectType.Expression;
    }

    #region PRIVATE METHODS

    private static bool IsBoolean(string value)
    {
        return value is "true" or "false";
    }

    private static bool IsNumber(string value)
    {
        return int.TryParse(value, out _)
            || double.TryParse(
                value,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out _
            );
    }

    private static bool IsStringLiteral(string value)
    {
        if (value.StartsWith('\'') && value.EndsWith('\'') && value.Length == 3)
        {
            return true;
        }

        if (value.StartsWith('\"') && value.EndsWith('\"'))
        {
            return !Operators.Any(value.Contains);
        }

        return false;
    }

    private static bool IsValidVariableOrStaticProperty(string name)
    {
        if (name.HasNoContent())
        {
            return false;
        }

        var parts = name.Split('.');

        return parts.All(IsValidIdentifier);
    }

    private static bool IsValidIdentifier(string identifier)
    {
        if (identifier.IsNullOrEmpty())
        {
            return false;
        }

        if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
        {
            return false;
        }

        return identifier.All(c => char.IsLetterOrDigit(c) || c == '_');
    }

    private static bool IsFunction(string subject)
    {
        var openParenIndex = subject.IndexOf('(');
        var closeParenIndex = subject.LastIndexOf(')');

        if (openParenIndex <= 0 || closeParenIndex <= openParenIndex)
        {
            return false;
        }

        var functionName = subject.SafeSubstring(0, openParenIndex);

        return IsValidVariableOrStaticProperty(functionName);
    }

    #endregion
}
