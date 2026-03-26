using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Exceptions;
using JAAvila.FluentOperations.Model;
using MediatR;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// MediatR pipeline behavior that automatically validates requests using Quality Blueprints.
/// Finds the first registered <see cref="IBlueprintValidator"/> that can validate
/// <typeparamref name="TRequest"/> and runs it before the handler is invoked.
/// </summary>
/// <remarks>
/// This behavior is AOT-safe: it depends on <see cref="IBlueprintValidator"/> via DI
/// and never uses <c>MakeGenericType</c> or <c>GetMethod</c> at runtime.
/// Register blueprints with the DI helpers (e.g. <c>AddBlueprint&lt;T&gt;</c>) so that
/// <see cref="IEnumerable{IBlueprintValidator}"/> is populated.
/// </remarks>
/// <typeparam name="TRequest">The request type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public class MediatRBlueprintBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IBlueprintValidator> _validators;
    private readonly IReadOnlyList<IAsyncBlueprintInterceptor> _interceptors;

    public MediatRBlueprintBehavior(
        IEnumerable<IBlueprintValidator> validators,
        IEnumerable<IAsyncBlueprintInterceptor>? interceptors = null)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        _interceptors = interceptors?.ToList() ?? (IReadOnlyList<IAsyncBlueprintInterceptor>)Array.Empty<IAsyncBlueprintInterceptor>();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var validator = FindValidator(typeof(TRequest));

        if (validator == null)
        {
            return await next();
        }

        QualityReport report;

        if (_interceptors.Count == 0)
        {
            report = await validator.ValidateAsync(request);
        }
        else
        {
            var ctx = new BlueprintInterceptionContext(request, typeof(TRequest), validator, "MediatR");
            report = await BlueprintInterceptorPipeline.ExecuteAsync(
                _interceptors, ctx, async instance => await validator.ValidateAsync(instance));
        }

        if (!report.IsValid)
        {
            throw new BlueprintValidationException(
                $"Validation failed for {typeof(TRequest).Name}",
                report
            );
        }

        return await next();
    }

    private IBlueprintValidator? FindValidator(Type requestType)
    {
        foreach (var validator in _validators)
        {
            if (validator.CanValidate(requestType))
            {
                return validator;
            }
        }

        return null;
    }
}

/// <summary>
/// Strongly typed MediatR pipeline behavior for requests whose blueprint validates
/// the request type directly (<c>TBlueprint : QualityBlueprint&lt;TRequest&gt;</c>).
/// Use when the blueprint's model type is <strong>exactly</strong> <typeparamref name="TRequest"/>.
/// </summary>
/// <remarks>
/// <para>
/// If your blueprint validates a base model and the request inherits from it, use
/// <see cref="MediatRBlueprintBehavior{TRequest, TResponse}"/> registered via
/// <see cref="MediatRExtensions.AddBlueprintBehavior{TRequest, TResponse, TModel, TBlueprint}"/>
/// or <see cref="MediatRExtensions.AddBlueprintValidation"/> instead.
/// </para>
/// <para>
/// Example — wrong (compile error): <c>QualityBlueprint&lt;User&gt;</c> cannot be used here if
/// <c>TRequest</c> is <c>CreateUserCommand</c>, because the constraint
/// <c>TBlueprint : QualityBlueprint&lt;TRequest&gt;</c> is not satisfied.<br/>
/// Example — correct: use the 4-generic overload
/// <c>AddBlueprintBehavior&lt;CreateUserCommand, string, User, UserBlueprint&gt;</c>.
/// </para>
/// </remarks>
/// <typeparam name="TRequest">The request type (must match the blueprint's model type exactly)</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
/// <typeparam name="TBlueprint">The blueprint type (must inherit <c>QualityBlueprint&lt;TRequest&gt;</c>)</typeparam>
public class MediatRBlueprintBehavior<TRequest, TResponse, TBlueprint>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TBlueprint : QualityBlueprint<TRequest>
{
    private readonly TBlueprint _blueprint;
    private readonly IReadOnlyList<IAsyncBlueprintInterceptor> _interceptors;

    public MediatRBlueprintBehavior(
        TBlueprint blueprint,
        IEnumerable<IAsyncBlueprintInterceptor>? interceptors = null)
    {
        _blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));
        _interceptors = interceptors?.ToList() ?? (IReadOnlyList<IAsyncBlueprintInterceptor>)Array.Empty<IAsyncBlueprintInterceptor>();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        QualityReport report;

        if (_interceptors.Count == 0)
        {
            report = await _blueprint.CheckAsync(request);
        }
        else
        {
            var validator = (IBlueprintValidator)_blueprint;
            var ctx = new BlueprintInterceptionContext(request, typeof(TRequest), validator, "MediatR");
            report = await BlueprintInterceptorPipeline.ExecuteAsync(
                _interceptors, ctx, async instance => await _blueprint.CheckAsync((TRequest)instance));
        }

        if (!report.IsValid)
        {
            throw new BlueprintValidationException(
                $"Validation failed for {typeof(TRequest).Name}",
                report
            );
        }

        return await next();
    }
}
