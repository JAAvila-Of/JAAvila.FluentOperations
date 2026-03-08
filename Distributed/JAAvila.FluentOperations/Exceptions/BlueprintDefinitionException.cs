namespace JAAvila.FluentOperations.Exceptions;

/// <summary>
/// Exception thrown when a QualityBlueprint definition contains errors
/// (e.g., For() without Test(), invalid property expressions).
/// </summary>
public class BlueprintDefinitionException : FluentOperationsException
{
    public BlueprintDefinitionException(string message) : base(message) { }
}
