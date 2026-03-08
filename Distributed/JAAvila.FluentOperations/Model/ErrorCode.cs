namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Convenience record for defining typed error codes.
/// Users can use this or simple string constants.
/// </summary>
/// <param name="Code">The error code identifier (e.g., "ERR001", "VALIDATION_NAME_REQUIRED")</param>
/// <param name="Description">Optional human-readable description of the error code</param>
public record ErrorCode(string Code, string? Description = null)
{
    /// <summary>
    /// Creates a new <see cref="ErrorCode"/> instance with the given code and optional description.
    /// </summary>
    /// <param name="code">The error code identifier (e.g., "ERR001").</param>
    /// <param name="description">Optional human-readable description of the error code.</param>
    /// <returns>A new <see cref="ErrorCode"/> instance.</returns>
    public static ErrorCode New(string code, string? description = null) => new(code, description);

    /// <summary>
    /// Implicit conversion from string to ErrorCode for convenience.
    /// </summary>
    public static implicit operator ErrorCode(string code) => new(code);

    /// <summary>
    /// Implicit conversion to string returns the Code.
    /// </summary>
    public static implicit operator string(ErrorCode errorCode) => errorCode.Code;

    /// <summary>
    /// Returns the error code string.
    /// </summary>
    public override string ToString() => Code;
}
