namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Controls how incoming messages are validated on client-streaming and duplex-streaming gRPC calls.
/// </summary>
public enum StreamValidationMode
{
    /// <summary>
    /// Validate only the first message received on the stream.
    /// Subsequent messages are passed through without validation.
    /// This is the default mode.
    /// </summary>
    FirstMessageOnly,

    /// <summary>
    /// Validate every message received on the stream.
    /// </summary>
    EveryMessage,

    /// <summary>
    /// Skip validation entirely for streaming calls.
    /// </summary>
    Skip
}
