using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Extension methods for integrating Quality Blueprints with ASP.NET Core Minimal APIs.
/// </summary>
public static class MinimalApiExtensions
{
    /// <summary>
    /// Adds blueprint validation to a Minimal API endpoint.
    /// The filter validates <typeparamref name="TModel"/> arguments using the specified
    /// <typeparamref name="TBlueprint"/> before the endpoint handler executes.
    /// </summary>
    /// <typeparam name="TModel">The model type to validate.</typeparam>
    /// <typeparam name="TBlueprint">The blueprint type. Must be registered in DI.</typeparam>
    /// <param name="builder">The route handler builder.</param>
    /// <returns>The route handler builder for further chaining.</returns>
    public static RouteHandlerBuilder WithBlueprint<TModel, TBlueprint>(
        this RouteHandlerBuilder builder)
        where TModel : notnull
        where TBlueprint : QualityBlueprint<TModel>
    {
        return builder.AddEndpointFilter<MinimalApiBlueprintFilter<TModel, TBlueprint>>();
    }
}
