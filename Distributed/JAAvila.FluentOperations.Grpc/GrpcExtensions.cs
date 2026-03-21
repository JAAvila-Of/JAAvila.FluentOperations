using Microsoft.Extensions.DependencyInjection;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Extension methods for integrating Quality Blueprint validation with gRPC services.
/// </summary>
public static class GrpcExtensions
{
    /// <summary>
    /// Registers <see cref="GrpcBlueprintInterceptor"/> as a transient service so it can be
    /// added to a gRPC service via <c>services.AddGrpc(o => o.Interceptors.Add&lt;GrpcBlueprintInterceptor&gt;())</c>.
    /// Uses default <see cref="GrpcBlueprintOptions"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddGrpcBlueprintValidation(this IServiceCollection services)
    {
        services.AddTransient<GrpcBlueprintInterceptor>();
        return services;
    }

    /// <summary>
    /// Registers <see cref="GrpcBlueprintInterceptor"/> as a transient service and configures
    /// <see cref="GrpcBlueprintOptions"/> via the provided delegate.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">A delegate to configure interceptor options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddGrpcBlueprintValidation(
        this IServiceCollection services,
        Action<GrpcBlueprintOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);

        var options = new GrpcBlueprintOptions();
        configureOptions(options);

        services.AddSingleton(options);
        services.AddTransient<GrpcBlueprintInterceptor>();
        return services;
    }
}
