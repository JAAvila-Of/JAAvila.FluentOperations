namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Captures the result of invoking an action for exception assertion purposes.
/// Contains the caught exception (if any) and metadata for error reporting.
/// </summary>
/// <typeparam name="TException">The expected exception type.</typeparam>
public class ExceptionCapture<TException>
    where TException : Exception
{
    /// <summary>
    /// The caught exception, cast to the expected type.
    /// </summary>
    public TException Exception { get; }

    /// <summary>
    /// The caller expression name used for error reporting.
    /// </summary>
    public string CallerName { get; }

    /// <summary>
    /// Whether the exception type must match exactly (ThrowExactly) or can be a derived type (Throw).
    /// </summary>
    public bool IsExactTypeMatch { get; }

    /// <summary>
    /// Represents the capture of an exception, including details about the exception type and the caller's context.
    /// </summary>
    /// <typeparam name="TException">The type of the captured exception. Must derive from <see cref="Exception"/>.</typeparam>
    public ExceptionCapture(TException exception, string callerName, bool isExactTypeMatch = false)
    {
        Exception = exception;
        CallerName = callerName;
        IsExactTypeMatch = isExactTypeMatch;
    }
}
