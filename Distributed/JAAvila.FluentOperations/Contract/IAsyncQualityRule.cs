namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Marker interface for quality rules that perform asynchronous validation.
/// Rules implementing this interface will have ValidateAsync() called
/// instead of Validate() during CheckAsync().
/// </summary>
internal interface IAsyncQualityRule;
