namespace JAAvila.FluentOperations.Exceptions;

/// <summary>
/// Base exception for all failures originating from the FluentOperations library.
/// All library-specific exceptions derive from this type.
/// </summary>
public class FluentOperationsException : Exception
{
    /// <summary>
    /// Initializes a new instance with the specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public FluentOperationsException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance with the specified error message and a reference to the inner exception that caused this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public FluentOperationsException(string message, Exception innerException)
        : base(message, innerException) { }
}
