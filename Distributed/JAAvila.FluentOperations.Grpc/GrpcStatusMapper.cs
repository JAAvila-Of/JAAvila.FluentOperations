using System.Text.Json;
using Grpc.Core;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Converts a <see cref="QualityReport"/> into an <see cref="RpcException"/>
/// with structured validation error metadata in gRPC trailers.
/// </summary>
public static class GrpcStatusMapper
{
    /// <summary>
    /// Converts a failed <see cref="QualityReport"/> to an <see cref="RpcException"/>.
    /// </summary>
    /// <param name="report">The quality report containing validation failures.</param>
    /// <param name="options">Options that control the status code and trailer inclusion.</param>
    /// <returns>An <see cref="RpcException"/> with validation error metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="report"/> is <see langword="null"/>.</exception>
    public static RpcException ToRpcException(QualityReport report, GrpcBlueprintOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(report);

        options ??= new GrpcBlueprintOptions();

        var errorDictionary = report.ToErrorDictionary();
        var errorCount = errorDictionary.Values.Sum(errors => errors.Length);
        var statusMessage = $"Validation failed with {errorCount} error(s).";

        var status = new Status(options.FailureStatusCode, statusMessage);
        var metadata = new Metadata();

        // Add per-property metadata entries for each Error-level failure.
        // gRPC metadata keys must be lowercase ASCII matching [a-z0-9_.-].
        // Property names such as "(root)" contain invalid characters and must be sanitized.
        foreach (var kvp in errorDictionary)
        {
            var key = $"validation-error-{SanitizeMetadataKey(kvp.Key)}";
            foreach (var message in kvp.Value)
            {
                metadata.Add(key, message);
            }
        }

        // Optionally add a binary trailer with the full JSON-serialized error dictionary.
        // Binary metadata keys MUST end in -bin per the gRPC spec.
        if (options.IncludeReportInTrailers && errorDictionary.Count > 0)
        {
            var json = JsonSerializer.Serialize(errorDictionary);
            metadata.Add(new Metadata.Entry("validation-errors-bin", System.Text.Encoding.UTF8.GetBytes(json)));
        }

        return new RpcException(status, metadata);
    }

    /// <summary>
    /// Sanitizes a property name for use as a gRPC metadata key.
    /// gRPC metadata keys must match [a-z0-9_.-]. Characters outside this set are removed.
    /// </summary>
    private static string SanitizeMetadataKey(string propertyName)
    {
        return propertyName
            .ToLowerInvariant()
            .Replace("(", "")
            .Replace(")", "")
            .Replace("[", "_")
            .Replace("]", "_");
    }
}
