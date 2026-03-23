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

    public MediatRBlueprintBehavior(IEnumerable<IBlueprintValidator> validators)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
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

        var report = await validator.ValidateAsync(request);

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

    public MediatRBlueprintBehavior(TBlueprint blueprint)
    {
        _blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var report = await _blueprint.CheckAsync(request);

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
