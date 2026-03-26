using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Action filter that automatically validates action parameters using Quality Blueprints.
/// Apply to controllers or actions to enable automatic validation.
/// </summary>
/// <remarks>
/// This filter is AOT-safe: it depends on <see cref="IBlueprintValidator"/> via DI
/// and never uses <c>MakeGenericType</c> or <c>GetMethod</c> at runtime.
/// Register blueprints with the DI helpers (e.g. <c>AddBlueprint&lt;T&gt;</c>) so that
/// <see cref="IEnumerable{IBlueprintValidator}"/> is populated.
/// </remarks>
public class BlueprintValidationFilter : IActionFilter
{
    private readonly IEnumerable<IBlueprintValidator> _validators;
    private readonly IReadOnlyList<IBlueprintInterceptor> _interceptors;

    public BlueprintValidationFilter(
        IEnumerable<IBlueprintValidator> validators,
        IEnumerable<IBlueprintInterceptor>? interceptors = null)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        _interceptors = interceptors?.ToList() ?? (IReadOnlyList<IBlueprintInterceptor>)Array.Empty<IBlueprintInterceptor>();
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value == null)
            {
                continue;
            }

            var type = argument.Value.GetType();
            var validator = FindValidator(type);

            if (validator == null)
            {
                continue;
            }

            QualityReport report;

            if (_interceptors.Count == 0)
            {
                report = validator.Validate(argument.Value);
            }
            else
            {
                var ctx = new BlueprintInterceptionContext(argument.Value, type, validator, "AspNetCore");
                report = BlueprintInterceptorPipeline.Execute(
                    _interceptors, ctx, instance => validator.Validate(instance));
            }

            if (report.IsValid)
            {
                continue;
            }

            context.Result = new BadRequestObjectResult(report.ToProblemDetails());
            return;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }

    private IBlueprintValidator? FindValidator(Type modelType)
    {
        foreach (var validator in _validators)
        {
            if (validator.CanValidate(modelType))
            {
                return validator;
            }
        }

        return null;
    }
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
    private readonly IReadOnlyList<IBlueprintInterceptor> _interceptors;

    public BlueprintValidationFilter(
        TBlueprint blueprint,
        IEnumerable<IBlueprintInterceptor>? interceptors = null)
    {
        _blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));
        _interceptors = interceptors?.ToList() ?? (IReadOnlyList<IBlueprintInterceptor>)Array.Empty<IBlueprintInterceptor>();
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value is not TModel model)
            {
                continue;
            }

            QualityReport report;

            if (_interceptors.Count == 0)
            {
                report = _blueprint.Check(model);
            }
            else
            {
                var validator = (IBlueprintValidator)_blueprint;
                var ctx = new BlueprintInterceptionContext(model!, typeof(TModel), validator, "AspNetCore");
                report = BlueprintInterceptorPipeline.Execute(
                    _interceptors, ctx, instance => _blueprint.Check((TModel)instance));
            }

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
