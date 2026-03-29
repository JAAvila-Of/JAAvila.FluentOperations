namespace JAAvila.FluentOperations.Exceptions;

/// <summary>
/// Exception thrown when the FluentOperations framework encounters
/// invalid configuration (e.g., invalid attribute values, incompatible options).
/// </summary>
public class ConfigurationException : FluentOperationsException
{
    /// <summary>
    /// Exception thrown when the FluentOperations framework encounters
    /// invalid configuration (e.g., invalid attribute values, incompatible options).
    /// </summary>
    public ConfigurationException(string message) : base(message) { }
}
