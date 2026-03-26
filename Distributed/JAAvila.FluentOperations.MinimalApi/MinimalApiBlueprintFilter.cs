using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Endpoint filter that validates request models using Quality Blueprints.
/// When validation fails, returns an RFC 7807 Problem Details response with status 400.
/// Only <see cref="Severity.Error"/> failures block the request; warnings and info-level
/// failures are ignored and the request proceeds normally.
/// </summary>
/// <typeparam name="TModel">The model type to validate from the endpoint arguments.</typeparam>
/// <typeparam name="TBlueprint">The blueprint type used to validate the model.</typeparam>
public class MinimalApiBlueprintFilter<TModel, TBlueprint> : IEndpointFilter
    where TModel : notnull
    where TBlueprint : QualityBlueprint<TModel>
{
    /// <summary>
    /// Invokes the endpoint filter. Searches for a <typeparamref name="TModel"/> instance
    /// in the endpoint arguments, validates it with the blueprint, and returns a
    /// ValidationProblem result if validation fails.
    /// </summary>
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        // 1. Search for TModel in the endpoint arguments
        TModel? model = default;

        foreach (var argument in context.Arguments)
        {
            if (argument is TModel typedModel)
            {
                model = typedModel;
                break;
            }
        }

        if (model is null)
        {
            return await next(context);
        }

        // 2. Resolve the blueprint from DI
        var blueprint = context.HttpContext.RequestServices.GetRequiredService<TBlueprint>();

        // 3. Execute async validation (with interceptor pipeline when interceptors are registered)
        var interceptors = context.HttpContext.RequestServices
            .GetServices<IAsyncBlueprintInterceptor>()
            .ToList();

        QualityReport report;

        if (interceptors.Count == 0)
        {
            report = await blueprint.CheckAsync(model);
        }
        else
        {
            var validator = (IBlueprintValidator)blueprint;
            var ctx = new BlueprintInterceptionContext(model, typeof(TModel), validator, "MinimalApi");
            report = await BlueprintInterceptorPipeline.ExecuteAsync(
                interceptors, ctx, async instance => await blueprint.CheckAsync((TModel)instance));
        }

        // 4. If invalid (has Error-severity failures), return ValidationProblem (RFC 7807)
        if (!report.IsValid)
        {
            var errors = report.Failures
                .Where(f => f.Severity == Severity.Error)
                .GroupBy(f => f.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(f => f.Message).ToArray());

            return TypedResults.ValidationProblem(errors);
        }

        // 5. Continue the pipeline (warnings and infos do not block)
        return await next(context);
    }
}
