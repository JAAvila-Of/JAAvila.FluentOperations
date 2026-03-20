using System.Reflection;
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

    /// <summary>
    /// Scans the specified assembly for all public, non-abstract classes that inherit from
    /// <see cref="QualityBlueprint{T}"/> and registers them as singletons.
    /// Each blueprint is registered both as its concrete type and as its
    /// <c>QualityBlueprint&lt;TModel&gt;</c> base type, enabling resolution by either type.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly to scan for blueprint implementations.</param>
    /// <returns>The service collection for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> or <paramref name="assembly"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Only public types returned by <see cref="Assembly.GetExportedTypes()"/> are considered.
    /// Abstract classes and interfaces are excluded.
    /// </para>
    /// <para>
    /// Blueprints with parameterized constructors are not supported by assembly scanning
    /// and must be registered manually using <see cref="AddBlueprint{TBlueprint}"/>
    /// or standard DI registration methods.
    /// </para>
    /// <para>
    /// The dual registration (concrete + base generic) ensures compatibility with
    /// <c>BlueprintValidationFilter</c> (ASP.NET Core) and <c>MediatRBlueprintBehavior</c>,
    /// which resolve blueprints via <c>typeof(QualityBlueprint&lt;&gt;).MakeGenericType(modelType)</c>.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Register all blueprints in the current assembly
    /// services.AddBlueprints(typeof(MyBlueprint).Assembly);
    /// </code>
    /// </example>
    public static IServiceCollection AddBlueprints(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        return AddBlueprints(services, assembly, _ => true);
    }

    /// <summary>
    /// Scans the specified assembly for public, non-abstract classes that inherit from
    /// <see cref="QualityBlueprint{T}"/> and match the given <paramref name="filter"/>,
    /// then registers them as singletons.
    /// Each blueprint is registered both as its concrete type and as its
    /// <c>QualityBlueprint&lt;TModel&gt;</c> base type, enabling resolution by either type.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly to scan for blueprint implementations.</param>
    /// <param name="filter">
    /// A predicate applied to each candidate type. Only types for which the predicate
    /// returns <c>true</c> are registered. Receives the concrete blueprint type.
    /// </param>
    /// <returns>The service collection for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/>, <paramref name="assembly"/>,
    /// or <paramref name="filter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Only public types returned by <see cref="Assembly.GetExportedTypes()"/> are considered.
    /// Abstract classes and interfaces are excluded before the filter is applied.
    /// </para>
    /// <para>
    /// Blueprints with parameterized constructors are not supported by assembly scanning
    /// and must be registered manually.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Register only blueprints whose name ends with "Blueprint"
    /// services.AddBlueprints(
    ///     typeof(MyBlueprint).Assembly,
    ///     t => t.Name.EndsWith("Blueprint"));
    ///
    /// // Register only blueprints in a specific namespace
    /// services.AddBlueprints(
    ///     typeof(MyBlueprint).Assembly,
    ///     t => t.Namespace?.StartsWith("MyApp.Validation") == true);
    /// </code>
    /// </example>
    public static IServiceCollection AddBlueprints(
        this IServiceCollection services,
        Assembly assembly,
        Func<Type, bool> filter
    )
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(filter);

        var blueprintTypes = assembly
            .GetExportedTypes()
            .Where(t => t is { IsAbstract: false, IsClass: true })
            .Where(IsBlueprintType)
            .Where(filter);

        foreach (var type in blueprintTypes)
        {
            // Register as concrete type (e.g., OrderBlueprint)
            services.AddSingleton(type);

            // Register as QualityBlueprint<TModel> for generic resolution
            // (used by BlueprintValidationFilter and MediatRBlueprintBehavior)
            var baseType = GetBlueprintBaseType(type);

            if (baseType is not null)
            {
                services.AddSingleton(baseType, sp => sp.GetRequiredService(type));
            }
        }

        return services;
    }

    /// <summary>
    /// Determines whether the specified type inherits (directly or indirectly)
    /// from <see cref="QualityBlueprint{T}"/>.
    /// </summary>
    private static bool IsBlueprintType(Type type)
    {
        var current = type.BaseType;

        while (current is not null)
        {
            if (current.IsGenericType
                && current.GetGenericTypeDefinition() == typeof(QualityBlueprint<>))
            {
                return true;
            }

            current = current.BaseType;
        }

        return false;
    }

    /// <summary>
    /// Walks the inheritance chain of <paramref name="type"/> to find the closed generic
    /// <c>QualityBlueprint&lt;TModel&gt;</c> base type.
    /// Returns <c>null</c> if the type does not inherit from <see cref="QualityBlueprint{T}"/>.
    /// </summary>
    private static Type? GetBlueprintBaseType(Type type)
    {
        var current = type.BaseType;

        while (current is not null)
        {
            if (current.IsGenericType
                && current.GetGenericTypeDefinition() == typeof(QualityBlueprint<>))
            {
                return current;
            }

            current = current.BaseType;
        }

        return null;
    }
}
