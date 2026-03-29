using JAAvila.FluentOperations.Contract;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JAAvila.FluentOperations.OpenApi;

/// <summary>
/// Extension methods for <see cref="SwaggerGenOptions"/> to integrate
/// FluentOperations Quality Blueprint validation with OpenAPI schema generation.
/// </summary>
public static class OpenApiExtensions
{
    /// <summary>
    /// Registers the <see cref="BlueprintSchemaFilter"/> with Swashbuckle so that
    /// OpenAPI schemas are automatically enriched with validation constraints derived
    /// from the provided <see cref="IBlueprintValidator"/> instances.
    /// </summary>
    /// <param name="options">The Swashbuckle <see cref="SwaggerGenOptions"/> to configure.</param>
    /// <param name="validators">
    /// The blueprint validators whose rules will be mapped to schema constraints.
    /// Typically resolved from DI via <c>IEnumerable&lt;IBlueprintValidator&gt;</c>.
    /// </param>
    /// <returns>The <paramref name="options"/> instance for fluent chaining.</returns>
    /// <example>
    /// <code>
    /// builder.Services.AddSwaggerGen(options =>
    /// {
    ///     options.AddFluentOperationsValidation(
    ///         app.Services.GetRequiredService&lt;IEnumerable&lt;IBlueprintValidator&gt;&gt;());
    /// });
    /// </code>
    /// </example>
    public static SwaggerGenOptions AddFluentOperationsValidation(
        this SwaggerGenOptions options,
        IEnumerable<IBlueprintValidator> validators
    )
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(validators);

        var filter = new BlueprintSchemaFilter(validators);
        options.SchemaFilter<BlueprintSchemaFilter>(() => filter);
        return options;
    }
}
