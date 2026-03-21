using Grpc.Core;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Configuration options for <see cref="GrpcBlueprintInterceptor"/>.
/// </summary>
public class GrpcBlueprintOptions
{
    /// <summary>
    /// The gRPC status code returned when validation fails.
    /// Defaults to <see cref="StatusCode.InvalidArgument"/>.
    /// </summary>
    public StatusCode FailureStatusCode { get; set; } = StatusCode.InvalidArgument;

    /// <summary>
    /// Controls how messages are validated on client-streaming and duplex-streaming calls.
    /// Defaults to <see cref="StreamValidationMode.FirstMessageOnly"/>.
    /// </summary>
    public StreamValidationMode StreamValidation { get; set; } = StreamValidationMode.FirstMessageOnly;

    /// <summary>
    /// When <see langword="true"/>, a binary trailer entry <c>validation-errors-bin</c>
    /// containing a JSON-serialized error dictionary is included in the <see cref="RpcException"/> metadata.
    /// Defaults to <see langword="true"/>.
    /// </summary>
    public bool IncludeReportInTrailers { get; set; } = true;
}
