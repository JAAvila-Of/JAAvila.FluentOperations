using Microsoft.Extensions.DependencyInjection;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Extension methods for integrating Quality Blueprints with dependency injection.
/// </summary>
public static class FluentOperationsDIExtensions
{
    /// <summary>
    /// Registers a Quality Blueprint for dependency injection.
    /// Blueprints are registered as singletons since they're stateless and reusable.
    /// </summary>
    /// <typeparam name="TBlueprint">The blueprint type to register</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBlueprint<TBlueprint>(this IServiceCollection services)
        where TBlueprint : class
    {
        return services.AddSingleton<TBlueprint>();
    }

    /// <summary>
    /// Registers multiple blueprints at once.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="blueprintTypes">Array of blueprint types to register</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBlueprints(
        this IServiceCollection services,
        params Type[] blueprintTypes
    )
    {
        foreach (var type in blueprintTypes)
        {
            services.AddSingleton(type);
        }

        return services;
    }
}
