namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Marker interface for conditional validation rules.
/// Rules implementing this interface receive the model instance
/// for condition evaluation before the actual validation.
/// </summary>
internal interface IConditionalRule;
