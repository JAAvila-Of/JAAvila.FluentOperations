using JAAvila.FluentOperations.Exceptions;
using JAAvila.FluentOperations.Model;
using MediatR;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// MediatR pipeline behavior that automatically validates requests using Quality Blueprints.
/// Walks up the type hierarchy to find blueprints registered for base types,
/// enabling scenarios where a Blueprint&lt;BaseModel&gt; validates a derived request type.
/// </summary>
/// <typeparam name="TRequest">The request type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public class MediatRBlueprintBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IServiceProvider _serviceProvider;

    public MediatRBlueprintBehavior(IServiceProvider serviceProvider)
    {
        _serviceProvider =
            serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        object? blueprint = null;
        Type? resolvedType = null;
        var currentType = typeof(TRequest);

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
            return await next();
        }

        var checkMethod = blueprint.GetType().GetMethod("CheckAsync", [resolvedType]);

        if (checkMethod == null)
        {
            return await next();
        }

        var report = await (Task<QualityReport>)checkMethod.Invoke(blueprint, [request])!;

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

/// <summary>
/// Strongly typed MediatR pipeline behavior for requests with specific blueprints.
/// Use when TRequest matches TBlueprint's model type exactly.
/// </summary>
/// <typeparam name="TRequest">The request type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
/// <typeparam name="TBlueprint">The blueprint type</typeparam>
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
