using Grpc.Core;
using Grpc.Core.Interceptors;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// gRPC server interceptor that automatically validates incoming requests using Quality Blueprints.
/// Finds the first registered <see cref="IBlueprintValidator"/> that can validate the request type
/// and runs it before the handler is invoked.
/// </summary>
/// <remarks>
/// <para>
/// This interceptor is AOT-safe: it depends on <see cref="IBlueprintValidator"/> via DI
/// and never uses <c>MakeGenericType</c> or <c>GetMethod</c> at runtime.
/// </para>
/// <para>
/// Register via <see cref="GrpcExtensions.AddGrpcBlueprintValidation(Microsoft.Extensions.DependencyInjection.IServiceCollection)"/> and configure gRPC
/// to use the interceptor via <c>services.AddGrpc(o => o.Interceptors.Add&lt;GrpcBlueprintInterceptor&gt;())</c>.
/// </para>
/// </remarks>
public class GrpcBlueprintInterceptor : Interceptor
{
    private readonly IEnumerable<IBlueprintValidator> _validators;
    private readonly GrpcBlueprintOptions _options;
    private readonly IReadOnlyList<IAsyncBlueprintInterceptor> _interceptors;

    /// <summary>
    /// Initializes a new instance of <see cref="GrpcBlueprintInterceptor"/>.
    /// </summary>
    /// <param name="validators">All registered blueprint validators.</param>
    /// <param name="options">Interceptor options. When <see langword="null"/>, defaults are used.</param>
    /// <param name="interceptors">Blueprint interceptors to run before/after validation.</param>
    public GrpcBlueprintInterceptor(
        IEnumerable<IBlueprintValidator> validators,
        GrpcBlueprintOptions? options = null,
        IEnumerable<IAsyncBlueprintInterceptor>? interceptors = null)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        _options = options ?? new GrpcBlueprintOptions();
        _interceptors = interceptors?.ToList() ?? (IReadOnlyList<IAsyncBlueprintInterceptor>)Array.Empty<IAsyncBlueprintInterceptor>();
    }

    /// <inheritdoc/>
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        await ValidateOrThrowAsync(request);
        return await continuation(request, context);
    }

    /// <inheritdoc/>
    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        if (_options.StreamValidation == StreamValidationMode.Skip)
        {
            return await continuation(requestStream, context);
        }

        var wrappedStream = new ValidatingStreamReader<TRequest>(requestStream, ValidateOrThrowAsync, _options.StreamValidation);
        return await continuation(wrappedStream, context);
    }

    /// <inheritdoc/>
    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        await ValidateOrThrowAsync(request);
        await continuation(request, responseStream, context);
    }

    /// <inheritdoc/>
    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        if (_options.StreamValidation == StreamValidationMode.Skip)
        {
            await continuation(requestStream, responseStream, context);
            return;
        }

        var wrappedStream = new ValidatingStreamReader<TRequest>(requestStream, ValidateOrThrowAsync, _options.StreamValidation);
        await continuation(wrappedStream, responseStream, context);
    }

    /// <summary>
    /// Finds the matching validator for <typeparamref name="T"/>, runs it, and throws
    /// an <see cref="RpcException"/> if the report is invalid. Passes through when no
    /// matching validator is registered.
    /// </summary>
    internal async Task ValidateOrThrowAsync<T>(T request)
    {
        if (request is null)
        {
            return;
        }

        var validator = FindValidator(typeof(T));

        if (validator is null)
        {
            return;
        }

        QualityReport report;

        if (_interceptors.Count == 0)
        {
            report = await validator.ValidateAsync(request);
        }
        else
        {
            var ctx = new BlueprintInterceptionContext(request, typeof(T), validator, "Grpc");
            report = await BlueprintInterceptorPipeline.ExecuteAsync(
                _interceptors, ctx, async instance => await validator.ValidateAsync(instance));
        }

        if (!report.IsValid)
        {
            throw GrpcStatusMapper.ToRpcException(report, _options);
        }
    }

    private IBlueprintValidator? FindValidator(Type requestType)
    {
        foreach (var validator in _validators)
        {
            if (validator.CanValidate(requestType))
            {
                return validator;
            }
        }

        return null;
    }
}
