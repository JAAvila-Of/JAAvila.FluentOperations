using JAAvila.FluentOperations.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Action filter that automatically validates action parameters using Quality Blueprints.
/// Apply to controllers or actions to enable automatic validation.
/// </summary>
public class BlueprintValidationFilter : IActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public BlueprintValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider =
            serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value == null)
            {
                continue;
            }

            object? blueprint = null;
            Type? resolvedType = null;
            var currentType = argument.Value.GetType();

            while (currentType != null && blueprint == null)
            {
                var blueprintType = typeof(QualityBlueprint<>).MakeGenericType(currentType);
                blueprint = _serviceProvider.GetService(blueprintType);

                if (blueprint != null)
                {
                    resolvedType = currentType;
                }

                currentType = currentType.BaseType;
            }

            if (blueprint == null || resolvedType == null)
            {
                continue;
            }

            var checkMethod = blueprint.GetType().GetMethod("Check", [resolvedType]);

            if (checkMethod == null)
            {
                continue;
            }

            var report = (QualityReport)checkMethod.Invoke(blueprint, [argument.Value])!;

            if (report.IsValid)
            {
                continue;
            }

            context.Result = new BadRequestObjectResult(report.ToProblemDetails());
            return;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}

/// <summary>
/// Attribute to enable blueprint validation on specific controllers or actions.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateWithBlueprintAttribute : ServiceFilterAttribute
{
    public ValidateWithBlueprintAttribute()
        : base(typeof(BlueprintValidationFilter)) { }
}

/// <summary>
/// Strongly typed action filter for validating specific model types.
/// </summary>
/// <typeparam name="TModel">The model type to validate</typeparam>
/// <typeparam name="TBlueprint">The blueprint type</typeparam>
public class BlueprintValidationFilter<TModel, TBlueprint> : IActionFilter
    where TBlueprint : QualityBlueprint<TModel>
{
    private readonly TBlueprint _blueprint;

    public BlueprintValidationFilter(TBlueprint blueprint)
    {
        _blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value is not TModel model)
            {
                continue;
            }

            var report = _blueprint.Check(model);

            if (report.IsValid)
            {
                continue;
            }

            context.Result = new BadRequestObjectResult(report.ToProblemDetails());
            return;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
