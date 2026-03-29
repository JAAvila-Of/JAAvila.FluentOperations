namespace JAAvila.FluentOperations.Exceptions;

/// <summary>
/// Exception thrown when a QualityBlueprint definition contains errors
/// (e.g., For() without Test(), invalid property expressions).
/// </summary>
public class BlueprintDefinitionException : FluentOperationsException
{
    /// <summary>
    /// Represents an exception thrown when a QualityBlueprint definition in the FluentOperations library
    /// contains errors, such as missing required methods or invalid property expressions.
    /// </summary>
    public BlueprintDefinitionException(string message) : base(message) { }
}
