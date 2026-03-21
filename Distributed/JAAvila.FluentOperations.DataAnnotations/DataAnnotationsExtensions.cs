using JAAvila.FluentOperations.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace JAAvila.FluentOperations.DataAnnotations;

/// <summary>
/// Dependency injection extension methods for registering DataAnnotations-derived blueprints.
/// </summary>
public static class DataAnnotationsExtensions
{
    /// <summary>
    /// Creates a <see cref="DataAnnotationsBlueprint{T}"/> whose rules are derived entirely
    /// from the DataAnnotations attributes on <typeparamref name="T"/>, and registers it as
    /// a singleton in the DI container.
    /// </summary>
    /// <typeparam name="T">The model type whose annotations are scanned.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="options">
    /// Optional mapping options.  When <c>null</c>, <see cref="AnnotationOptions.Default"/> is used.
    /// </param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddDataAnnotationsBlueprint<T>(
        this IServiceCollection services,
        AnnotationOptions? options = null
    )
        where T : notnull
    {
        var blueprint = DataAnnotationsBlueprint<T>.FromAnnotations(options);
        services.AddSingleton(blueprint);
        services.AddSingleton<IBlueprintValidator>(blueprint);
        return services;
    }
}
