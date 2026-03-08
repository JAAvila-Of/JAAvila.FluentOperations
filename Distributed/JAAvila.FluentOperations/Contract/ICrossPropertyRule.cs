namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Marker interface for cross-property validation rules.
/// Rules implementing this interface receive the full model instance
/// via SetValue() instead of just the property value.
/// </summary>
internal interface ICrossPropertyRule;
