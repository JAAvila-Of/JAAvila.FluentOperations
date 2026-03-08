using JAAvila.SafeTypes.Attributes;

namespace JAAvila.FluentOperations.Common;

/// <summary>
/// Represents the type of subject on which nested operations will be performed, the subject is
/// captured each time the "Test" function of <see cref="TestExtension"/> is called.
/// </summary>
internal enum SubjectType
{
    /// <summary>
    /// Represents a subject type associated with an empty or null value.
    /// This is used when no valid subject is provided or when the subject
    /// string is null or empty during the operation initialization process.
    /// </summary>
    [EnumStringValue("")]
    Void,

    /// <summary>
    /// Represents a subject type associated with primitive values such as boolean, numeric,
    /// or string literals. This is used when the subject refers to fundamental atomic data
    /// types during the operation initialization process.
    /// </summary>
    [EnumStringValue("primitive")]
    Primitive,

    /// <summary>
    /// Represents a subject type associated with a variable or static property.
    /// This is used when the subject corresponds to a valid identifier, such as
    /// variable names or static property access expressions, during operations.
    /// </summary>
    [EnumStringValue("variable")]
    Variable,

    /// <summary>
    /// Represents a subject type identified as a function.
    /// This is used when the subject string corresponds to a valid function format.
    /// Functions are typically represented by a name followed by parentheses,
    /// optionally containing arguments.
    /// </summary>
    [EnumStringValue("function")]
    Function,

    /// <summary>
    /// Represents a subject type that defines a mathematical or logical expression.
    /// This type is used when the subject cannot be categorized as a primitive,
    /// variable, or function. It denotes complex expressions that are evaluated
    /// during the operation process.
    /// </summary>
    [EnumStringValue("expression")]
    Expression
}
