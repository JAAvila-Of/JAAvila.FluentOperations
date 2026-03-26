using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Provides context for blueprint interception, carrying the instance being validated,
/// its type, and a property bag for cross-interceptor communication.
/// </summary>
public class BlueprintInterceptionContext
{
    /// <summary>
    /// The object instance about to be (or already) validated.
    /// In <c>BeforeValidation</c>, can be replaced to validate a modified copy.
    /// </summary>
    public object Instance { get; set; }

    /// <summary>
    /// The runtime type of the object being validated.
    /// </summary>
    public Type ModelType { get; }

    /// <summary>
    /// The blueprint validator that will execute (or has executed) validation.
    /// </summary>
    public IBlueprintValidator Validator { get; }

    /// <summary>
    /// Identifies the integration source: "AspNetCore", "MinimalApi", "MediatR", "Grpc".
    /// Allows interceptors to behave differently per integration.
    /// </summary>
    public string IntegrationSource { get; }

    /// <summary>
    /// Arbitrary properties for cross-interceptor or cross-phase communication.
    /// Example: a "before" interceptor stores a Stopwatch, an "after" interceptor reads it.
    /// </summary>
    public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

    public BlueprintInterceptionContext(
        object instance,
        Type modelType,
        IBlueprintValidator validator,
        string integrationSource)
    {
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(modelType);
        ArgumentNullException.ThrowIfNull(validator);
        ArgumentNullException.ThrowIfNull(integrationSource);

        Instance = instance;
        ModelType = modelType;
        Validator = validator;
        IntegrationSource = integrationSource;
    }
}
