namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Marker + contract interface for rules that require access to the root model instance
/// during evaluation. The evaluation loop calls <see cref="SetModelInstance"/> before
/// <see cref="IQualityRule.Validate"/>/<see cref="IQualityRule.ValidateAsync"/>.
/// </summary>
/// <remarks>
/// This interface is shared between:
/// <list type="bullet">
///   <item>Gap 4: <c>ModelAwarePredicateRule</c> / <c>ModelAwareAsyncPredicateRule</c> (cross-property predicates)</item>
///   <item>Gap 2: Dynamic message rules that format messages using model properties</item>
/// </list>
/// Unlike <see cref="IConditionalRule"/> (which gates execution), <c>IModelAwareRule</c>
/// provides the model for use WITHIN validation logic.
/// </remarks>
internal interface IModelAwareRule
{
    /// <summary>
    /// Injects the root model instance before rule evaluation.
    /// Called by the Check/CheckAsync evaluation loops.
    /// </summary>
    /// <param name="model">The root model instance being validated.</param>
    void SetModelInstance(object model);
}
