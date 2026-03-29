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
    /// Adds a strongly typed blueprint behavior for a request type.
    /// The blueprint MUST validate the request type directly
    /// (<typeparamref name="TBlueprint"/> must inherit <c>QualityBlueprint&lt;TRequest&gt;</c>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this overload when you have a blueprint that models the request directly, e.g.:
    /// <code>
    /// // Blueprint: public class CreateOrderBlueprint: QualityBlueprint&lt;CreateOrderCommand&gt;
    /// services.AddBlueprintBehavior&lt;CreateOrderCommand, OrderResult, CreateOrderBlueprint&gt;()
    /// </code>
    /// </para>
    /// <para>
    /// If your blueprint validates a base model and the request derives from it, use the
    /// 4-generic overload <see cref="AddBlueprintBehavior{TRequest, TResponse, TModel, TBlueprint}"/> instead:
    /// <code>
    /// // Blueprint: public class UserBlueprint: QualityBlueprint&lt;User&gt;
    /// // Request:   public class CreateUserCommand: User, IRequest&lt;string&gt;
    /// services.AddBlueprintBehavior&lt;CreateUserCommand, string, User, UserBlueprint&gt;();
    /// </code>
    /// </para>
    /// <para>
    /// Alternatively, use <see cref="AddBlueprintValidation"/> for automatic blueprint discovery
    /// at runtime via <see cref="IBlueprintValidator.CanValidate"/>.
    /// </para>
    /// </remarks>
    /// <typeparam name="TRequest">The request type (must match the blueprint's model type)</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <typeparam name="TBlueprint">The blueprint type (must inherit <c>QualityBlueprint&lt;TRequest&gt;</c>)</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBlueprintBehavior<TRequest, TResponse, TBlueprint>(
        this IServiceCollection services
    )
        where TRequest : notnull
        where TBlueprint : QualityBlueprint<TRequest>
    {
        services.AddTransient<
            IPipelineBehavior<TRequest, TResponse>,
            MediatRBlueprintBehavior<TRequest, TResponse, TBlueprint>
        >();
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
        where TModel : notnull
    {
        services.AddSingleton<QualityBlueprint<TModel>, TBlueprint>();
        services.AddSingleton<IBlueprintValidator>(
            sp => sp.GetRequiredService<QualityBlueprint<TModel>>()
        );
        services.AddTransient<
            IPipelineBehavior<TRequest, TResponse>,
            MediatRBlueprintBehavior<TRequest, TResponse>
        >();
        return services;
    }
}
