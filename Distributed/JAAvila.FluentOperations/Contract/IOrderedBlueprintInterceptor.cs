namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Optional interface for interceptors that need explicit ordering.
/// Lower values execute first in BeforeValidation, last in AfterValidation (onion model).
/// The default order for interceptors without this interface is 0.
/// </summary>
public interface IOrderedBlueprintInterceptor
{
    /// <summary>
    /// Execution order. Lower values execute first in BeforeValidation.
    /// AfterValidation executes in reverse order (innermost interceptor's After runs first).
    /// </summary>
    int Order { get; }
}
