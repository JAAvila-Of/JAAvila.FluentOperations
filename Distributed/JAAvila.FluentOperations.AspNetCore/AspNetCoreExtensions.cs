using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Extension methods for integrating Quality Blueprints with ASP.NET Core.
/// </summary>
public static class AspNetCoreExtensions
{
    /// <summary>
    /// Adds a global blueprint validation filter to all controllers.
    /// All action parameters will be validated using their corresponding blueprints.
    /// </summary>
    /// <param name="options">The MVC options</param>
    /// <returns>The MVC options for chaining</returns>
    public static MvcOptions AddBlueprintValidation(this MvcOptions options)
    {
        options.Filters.Add<BlueprintValidationFilter>();
        return options;
    }

    /// <summary>
    /// Adds blueprint validation services to the DI container.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBlueprintValidationFilter(this IServiceCollection services)
    {
        services.AddScoped<BlueprintValidationFilter>();
        return services;
    }

    /// <summary>
    /// Adds a specific blueprint validation filter for a model type.
    /// </summary>
    /// <typeparam name="TModel">The model type</typeparam>
    /// <typeparam name="TBlueprint">The blueprint type</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBlueprintFilter<TModel, TBlueprint>(
        this IServiceCollection services
    )
        where TBlueprint : QualityBlueprint<TModel>
        where TModel : notnull
    {
        services.AddScoped<BlueprintValidationFilter<TModel, TBlueprint>>();
        return services;
    }

    /// <summary>
    /// Configures MVC to use blueprint validation and registers all necessary services.
    /// </summary>
    /// <param name="builder">The MVC builder</param>
    /// <returns>The MVC builder for chaining</returns>
    public static IMvcBuilder AddBlueprintValidation(this IMvcBuilder builder)
    {
        builder.Services.AddBlueprintValidationFilter();
        builder.AddMvcOptions(options =>
        {
            options.AddBlueprintValidation();
        });
        return builder;
    }
}
