using JAAvila.FluentOperations.Contract;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Extension methods for integrating Quality Blueprints with MediatR.
/// </summary>
public static class MediatRExtensions
{
    /// <summary>
    /// Adds automatic blueprint validation to the MediatR pipeline.
    /// All requests will be validated using their corresponding blueprints before reaching handlers.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBlueprintValidation(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatRBlueprintBehavior<,>));
        return services;
    }

    /// <summary>
    /// Adds a specific blueprint behavior for a request type.
    /// Use when the blueprint's model type matches the request type exactly.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <typeparam name="TBlueprint">The blueprint type</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBlueprintBehavior<TRequest, TResponse, TBlueprint>(
        this IServiceCollection services
    )
        where TRequest : notnull
        where TBlueprint : QualityBlueprint<TRequest>
    {
        services.AddTransient<IPipelineBehavior<TRequest, TResponse>, MediatRBlueprintBehavior<TRequest, TResponse, TBlueprint>>();
        return services;
    }

    /// <summary>
    /// Adds a blueprint behavior where the blueprint validates a base model type
    /// and the request is a derived type. Registers the generic behavior that
    /// walks the type hierarchy to find the matching blueprint.
    /// Also registers the blueprint as <see cref="IBlueprintValidator"/> for AOT-safe resolution.
    /// </summary>
    /// <typeparam name="TRequest">The request type (derived from TModel)</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <typeparam name="TModel">The base model type that the blueprint validates</typeparam>
    /// <typeparam name="TBlueprint">The blueprint type</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBlueprintBehavior<TRequest, TResponse, TModel, TBlueprint>(
        this IServiceCollection services
    )
        where TRequest : class, TModel
        where TBlueprint : QualityBlueprint<TModel>
    {
        services.AddSingleton<QualityBlueprint<TModel>, TBlueprint>();
        services.AddSingleton<IBlueprintValidator>(sp => (IBlueprintValidator)sp.GetRequiredService<QualityBlueprint<TModel>>());
        services.AddTransient<IPipelineBehavior<TRequest, TResponse>, MediatRBlueprintBehavior<TRequest, TResponse>>();
        return services;
    }
}
