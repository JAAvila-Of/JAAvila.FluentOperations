using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.DataAnnotations;

/// <summary>
/// Options that control how DataAnnotations attributes are mapped to Blueprint validation rules.
/// </summary>
public class AnnotationOptions
{
    /// <summary>
    /// Gets or sets the default severity applied to rules generated from DataAnnotations attributes.
    /// Defaults to <see cref="Severity.Error"/>.
    /// </summary>
    public Severity DefaultSeverity { get; set; } = Severity.Error;

    /// <summary>
    /// Gets or sets an optional prefix prepended to auto-generated error codes.
    /// When <c>null</c>, no error code is assigned unless the annotation provides one.
    /// </summary>
    public string? ErrorCodePrefix { get; set; } = null;

    /// <summary>
    /// Gets or sets a value indicating whether <see cref="System.ComponentModel.DataAnnotations.RequiredAttribute"/>
    /// annotations are ignored during mapping.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool IgnoreRequired { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the <c>ErrorMessage</c> defined on the annotation
    /// is used as the custom message for the generated rule.
    /// Defaults to <c>true</c>.
    /// </summary>
    public bool UseAnnotationErrorMessages { get; set; } = true;

    /// <summary>
    /// Gets or sets the set of property names that are excluded from annotation mapping.
    /// Property names are case-sensitive and match the C# member name.
    /// </summary>
    public HashSet<string> ExcludedProperties { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether properties declared on base types
    /// are included in annotation mapping.
    /// Defaults to <c>true</c>.
    /// </summary>
    public bool IncludeInheritedProperties { get; set; } = true;

    /// <summary>
    /// Gets a default <see cref="AnnotationOptions"/> instance with all properties at their default values.
    /// </summary>
    public static AnnotationOptions Default { get; } = new();
}
