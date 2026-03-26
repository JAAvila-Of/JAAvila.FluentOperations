using System.Reflection;
using JAAvila.FluentOperations.Contract;
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
    /// Also registers the blueprint as <see cref="IBlueprintValidator"/> when applicable,
    /// enabling AOT-safe resolution by <c>BlueprintValidationFilter</c> and <c>MediatRBlueprintBehavior</c>.
    /// </summary>
    /// <typeparam name="TBlueprint">The blueprint type to register</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBlueprint<TBlueprint>(this IServiceCollection services)
        where TBlueprint : class
    {
        services.AddSingleton<TBlueprint>();
        if (typeof(IBlueprintValidator).IsAssignableFrom(typeof(TBlueprint)))
            services.AddSingleton<IBlueprintValidator>(sp => (IBlueprintValidator)sp.GetRequiredService<TBlueprint>());
        return services;
    }

    /// <summary>
    /// Registers multiple blueprints at once.
    /// Also registers each blueprint as <see cref="IBlueprintValidator"/> when applicable.
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
            if (typeof(IBlueprintValidator).IsAssignableFrom(type))
                services.AddSingleton(typeof(IBlueprintValidator), sp => (IBlueprintValidator)sp.GetRequiredService(type));
        }

        return services;
    }

    /// <summary>
    /// Scans the specified assembly for all public, non-abstract classes that inherit from
    /// <see cref="QualityBlueprint{T}"/> and registers them as singletons.
    /// Each blueprint is registered both as its concrete type and as its
    /// <c>QualityBlueprint&lt;TModel&gt;</c> base type, enabling resolution by either type.
    /// Also registers each blueprint as <see cref="IBlueprintValidator"/> for AOT-safe filter resolution.
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
    /// <c>BlueprintValidationFilter</c> (ASP.NET Core) and <c>MediatRBlueprintBehavior</c>.
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
    /// Also registers each blueprint as <see cref="IBlueprintValidator"/> for AOT-safe filter resolution.
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
            var baseType = GetBlueprintBaseType(type);

            if (baseType is not null)
            {
                services.AddSingleton(baseType, sp => sp.GetRequiredService(type));
            }

            // Register as IBlueprintValidator for AOT-safe filter/behavior resolution
            services.AddSingleton(typeof(IBlueprintValidator), sp => (IBlueprintValidator)sp.GetRequiredService(type));
        }

        return services;
    }

    /// <summary>
    /// Registers a <see cref="CompositeBlueprint{T}"/> that composes the specified blueprint types.
    /// Each blueprint type is registered as a singleton (by concrete type only) if not already registered.
    /// The composite itself is registered as a singleton and as <see cref="IBlueprintValidator"/>,
    /// making it discoverable by integration filters.
    /// </summary>
    /// <typeparam name="T">The model type validated by all composed blueprints.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="blueprintTypes">
    /// The concrete blueprint types to compose. Each type must implement <see cref="IBlueprintValidator"/>.
    /// Must contain at least one type.
    /// </param>
    /// <returns>The service collection for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> or <paramref name="blueprintTypes"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="blueprintTypes"/> is empty.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Individual blueprint types are registered only as their concrete type — NOT as
    /// <see cref="IBlueprintValidator"/>. Only the composite is registered as
    /// <see cref="IBlueprintValidator"/> so that integration filters find the composite
    /// (and its merged report) rather than an individual blueprint for type <typeparamref name="T"/>.
    /// </para>
    /// <para>
    /// Do NOT also call <see cref="AddBlueprint{TBlueprint}"/> for blueprints that are part of a
    /// composite, as that would register them as <see cref="IBlueprintValidator"/> and cause
    /// filters to find the individual blueprint before the composite.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddCompositeBlueprint<T>(
        this IServiceCollection services,
        params Type[] blueprintTypes)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(blueprintTypes);

        if (blueprintTypes.Length == 0)
            throw new ArgumentException(
                "At least one blueprint type is required.", nameof(blueprintTypes));

        // Register each individual blueprint as its concrete type only (not as IBlueprintValidator).
        foreach (var type in blueprintTypes)
        {
            if (!services.Any(sd => sd.ServiceType == type))
            {
                services.AddSingleton(type);
            }
        }

        // Register the composite as its concrete type (Singleton).
        services.AddSingleton(sp =>
        {
            var validators = blueprintTypes
                .Select(t => (IBlueprintValidator)sp.GetRequiredService(t))
                .ToList();
            return new CompositeBlueprint<T>(validators);
        });

        // Register the composite as IBlueprintValidator for filter/behavior discovery.
        services.AddSingleton<IBlueprintValidator>(sp =>
            sp.GetRequiredService<CompositeBlueprint<T>>());

        return services;
    }

    /// <summary>
    /// Registers a <see cref="CompositeBlueprint{T}"/> using a factory delegate that receives
    /// the <see cref="IServiceProvider"/> for custom blueprint resolution.
    /// The composite is registered as a singleton and as <see cref="IBlueprintValidator"/>.
    /// </summary>
    /// <typeparam name="T">The model type validated by all composed blueprints.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="factory">
    /// A factory that, given the <see cref="IServiceProvider"/>, returns the
    /// <see cref="IBlueprintValidator"/> instances to compose. Must return at least one validator.
    /// </param>
    /// <returns>The service collection for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> or <paramref name="factory"/> is <see langword="null"/>.
    /// </exception>
    public static IServiceCollection AddCompositeBlueprint<T>(
        this IServiceCollection services,
        Func<IServiceProvider, IEnumerable<IBlueprintValidator>> factory)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(factory);

        // Register the composite as its concrete type (Singleton).
        services.AddSingleton(sp => new CompositeBlueprint<T>(factory(sp)));

        // Register the composite as IBlueprintValidator for filter/behavior discovery.
        services.AddSingleton<IBlueprintValidator>(sp =>
            sp.GetRequiredService<CompositeBlueprint<T>>());

        return services;
    }

    /// <summary>
    /// Registers a blueprint interceptor as a singleton.
    /// If <typeparamref name="TInterceptor"/> implements both <see cref="IBlueprintInterceptor"/>
    /// and <see cref="IAsyncBlueprintInterceptor"/>, it is registered under both interfaces.
    /// If it implements only <see cref="IBlueprintInterceptor"/> (sync), a
    /// <see cref="SyncToAsyncInterceptorAdapter"/> is automatically registered as
    /// <see cref="IAsyncBlueprintInterceptor"/> so sync interceptors work in async filters.
    /// Interceptors are invoked in registration order unless they implement
    /// <see cref="IOrderedBlueprintInterceptor"/>.
    /// </summary>
    public static IServiceCollection AddBlueprintInterceptor<TInterceptor>(this IServiceCollection services)
        where TInterceptor : class
    {
        var implementsSync = typeof(IBlueprintInterceptor).IsAssignableFrom(typeof(TInterceptor));
        var implementsAsync = typeof(IAsyncBlueprintInterceptor).IsAssignableFrom(typeof(TInterceptor));

        if (implementsSync)
            services.AddSingleton(typeof(IBlueprintInterceptor), typeof(TInterceptor));

        if (implementsAsync)
            services.AddSingleton(typeof(IAsyncBlueprintInterceptor), typeof(TInterceptor));

        // If only sync: auto-register an adapter so async filters can pick it up
        if (implementsSync && !implementsAsync)
        {
            services.AddSingleton<IAsyncBlueprintInterceptor>(sp =>
                new SyncToAsyncInterceptorAdapter(
                    (IBlueprintInterceptor)sp.GetRequiredService(typeof(TInterceptor))));
        }

        return services;
    }

    /// <summary>
    /// Registers a blueprint interceptor instance as a singleton.
    /// If the instance also implements <see cref="IAsyncBlueprintInterceptor"/>, it is registered
    /// under both interfaces. If it implements only <see cref="IBlueprintInterceptor"/> (sync),
    /// a <see cref="SyncToAsyncInterceptorAdapter"/> is automatically registered.
    /// </summary>
    public static IServiceCollection AddBlueprintInterceptor(
        this IServiceCollection services,
        IBlueprintInterceptor interceptor)
    {
        ArgumentNullException.ThrowIfNull(interceptor);

        services.AddSingleton(interceptor);

        if (interceptor is IAsyncBlueprintInterceptor asyncInterceptor)
            services.AddSingleton(asyncInterceptor);
        else
            services.AddSingleton<IAsyncBlueprintInterceptor>(new SyncToAsyncInterceptorAdapter(interceptor));

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
