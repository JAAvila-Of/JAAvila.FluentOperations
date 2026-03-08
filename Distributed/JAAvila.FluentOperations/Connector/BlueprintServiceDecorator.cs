using JAAvila.FluentOperations.Exceptions;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Connector;

/// <summary>
/// Decorator that automatically validates service method parameters using Quality Blueprints.
/// Works with Scrutor's decoration feature to intercept service calls and validate inputs.
/// </summary>
/// <typeparam name="TService">The service interface type</typeparam>
/// <typeparam name="TModel">The model type to validate</typeparam>
/// <typeparam name="TBlueprint">The blueprint type for validation</typeparam>
public class BlueprintServiceDecorator<TService, TModel, TBlueprint>
    where TBlueprint : QualityBlueprint<TModel>, new()
{
    private readonly TService _inner;
    private readonly TBlueprint _blueprint;

    protected BlueprintServiceDecorator(TService inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        _blueprint = new TBlueprint();
    }

    /// <summary>
    /// Validates a model using the configured blueprint before proceeding with the operation.
    /// </summary>
    /// <param name="model">The model to validate</param>
    /// <returns>Quality report with validation results</returns>
    /// <exception cref="BlueprintValidationException">Thrown when validation fails</exception>
    protected QualityReport ValidateModel(TModel model)
    {
        var report = _blueprint.Check(model);

        if (!report.IsValid)
        {
            throw new BlueprintValidationException(
                $"Validation failed for {typeof(TModel).Name}",
                report
            );
        }

        return report;
    }

    /// <summary>
    /// Gets the inner service instance being decorated.
    /// </summary>
    protected TService Inner => _inner;

    /// <summary>
    /// Gets the blueprint instance used for validation.
    /// </summary>
    protected TBlueprint Blueprint => _blueprint;
}
