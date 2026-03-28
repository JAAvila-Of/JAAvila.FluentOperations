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

    /// <summary>
    /// An action filter responsible for automatically validating action parameters
    /// using the Quality Blueprints framework. This filter ensures that the incoming
    /// request data complies with the set validation rules defined in the blueprint validators.
    /// </summary>
    /// <remarks>
    /// This filter is safe for Ahead-of-Time (AOT) compilation and relies on dependency injection
    /// for getting instances of <see cref="IBlueprintValidator"/> and optional <see cref="IBlueprintInterceptor"/>.
    /// It does not use reflection constructs such as <c>MakeGenericType</c> or <c>GetMethod</c>.
    /// To use this filter, register the blueprint validators with dependency injection (e.g., using
    /// custom helpers like <c>AddBlueprint&lt;T&gt;</c>) to ensure that all necessary validators are available
    /// in the service collection.
    /// </remarks>
    public BlueprintValidationFilter(
        IEnumerable<IBlueprintValidator> validators,
        IEnumerable<IBlueprintInterceptor>? interceptors = null
    )
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        _interceptors = interceptors?.ToList() ?? (IReadOnlyList<IBlueprintInterceptor>)[];
    }

    /// <summary>
    /// Executes custom logic before an action method is called. Validates action parameters
    /// based on Quality Blueprints and sets the HTTP response to a bad request if validation fails.
    /// </summary>
    /// <param name="context">
    /// The context for the executing action, which includes information about the action method,
    /// its controller, route data, and the action arguments.
    /// </param>
    /// <remarks>
    /// If any action argument fails validation, the execution flow is terminated, and an
    /// HTTP 400 Bad Request is returned with the validation failures in the response body.
    /// The validation is performed using registered <see cref="IBlueprintValidator"/> instances
    /// and optional <see cref="IBlueprintInterceptor"/> pipelines if interceptors are configured.
    /// </remarks>
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
                var ctx = new BlueprintInterceptionContext(
                    argument.Value,
                    type,
                    validator,
                    "AspNetCore"
                );
                report = BlueprintInterceptorPipeline.Execute(
                    _interceptors,
                    ctx,
                    instance => validator.Validate(instance)
                );
            }

            if (report.IsValid)
            {
                continue;
            }

            context.Result = new BadRequestObjectResult(report.ToProblemDetails());
            return;
        }
    }

    /// <summary>
    /// Provides logic to be executed after an action has been executed within the request pipeline.
    /// This method is primarily intended for post-processing logic and final validation checks
    /// using the Quality Blueprints framework.
    /// </summary>
    /// <param name="context">
    /// The <see cref="ActionExecutedContext"/> containing information about the executed action,
    /// including the result, controller, and request data. It provides access to the action's
    /// execution context and any exceptions that might have occurred during the action's execution.
    /// </param>
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
    /// <summary>
    /// Attribute that applies the <see cref="BlueprintValidationFilter"/> to controllers or actions.
    /// This attribute enables automatic validation of action parameters using Quality Blueprints
    /// for incoming requests.
    /// </summary>
    /// <remarks>
    /// The attribute leverages dependency injection to integrate with the <see cref="BlueprintValidationFilter"/>
    /// and ensures that validation logic defined in the associated blueprints is enforced.
    /// To use this attribute, register the necessary blueprints and their validators
    /// in the dependency injection container.
    /// </remarks>
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
    where TModel : notnull
{
    private readonly TBlueprint _blueprint;
    private readonly IReadOnlyList<IBlueprintInterceptor> _interceptors;

    /// <summary>
    /// An action filter responsible for validating specific model types using a strongly typed blueprint.
    /// This filter ensures compliance with the validation rules defined in the blueprint for the given model type.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated. Must be a non-nullable reference or value type.</typeparam>
    /// <typeparam name="TBlueprint">The type of the blueprint used for validation. Must inherit from <see cref="QualityBlueprint{TModel}"/>.</typeparam>
    /// <remarks>
    /// This filter is designed to integrate seamlessly with the JAAvila Fluent Operations framework by performing validation
    /// when an action is being executed. It leverages dependency injection to get the blueprint and optional interceptors.
    /// To use this filter, ensure that the necessary blueprint and interceptors are registered in the dependency injection
    /// container. During action execution:
    /// - The <c>OnActionExecuting</c> method runs the blueprint validation.
    /// - The <c>OnActionExecuted</c> method handles post-action execution logic.
    /// If <see cref="IBlueprintInterceptor"/> implementations are provided, they are executed before validation to enable
    /// fine-grained control over the validation behavior.
    /// </remarks>
    public BlueprintValidationFilter(
        TBlueprint blueprint,
        IEnumerable<IBlueprintInterceptor>? interceptors = null
    )
    {
        _blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));
        _interceptors = interceptors?.ToList() ?? (IReadOnlyList<IBlueprintInterceptor>)[];
    }

    /// <summary>
    /// Executes before the action method is called and validates action arguments
    /// using the associated blueprint validation logic.
    /// </summary>
    /// <param name="context">
    /// The context for the action executing filter. Provides access to action arguments
    /// and allows modifications to the result if validation fails.
    /// </param>
    /// <remarks>
    /// This method is responsible for validating models passed as action parameters
    /// against the rules defined in the associated blueprint. If validation fails,
    /// it sets the result as a <see cref="BadRequestObjectResult"/> containing the validation
    /// errors represented as problem details.
    /// </remarks>
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
                IBlueprintValidator validator = _blueprint;
                var ctx = new BlueprintInterceptionContext(
                    model,
                    typeof(TModel),
                    validator,
                    "AspNetCore"
                );
                report = BlueprintInterceptorPipeline.Execute(
                    _interceptors,
                    ctx,
                    instance => _blueprint.Check((TModel)instance)
                );
            }

            if (report.IsValid)
            {
                continue;
            }

            context.Result = new BadRequestObjectResult(report.ToProblemDetails());
            return;
        }
    }

    /// <summary>
    /// This method is invoked after the action method has executed. It provides an opportunity
    /// to perform actions or apply logic after the execution of an action in the MVC request pipeline.
    /// </summary>
    /// <param name="context">
    /// The <see cref="ActionExecutedContext"/> containing information about the executed action,
    /// including the action result, exception details (if any), and HTTP context information.
    /// </param>
    public void OnActionExecuted(ActionExecutedContext context) { }
}
