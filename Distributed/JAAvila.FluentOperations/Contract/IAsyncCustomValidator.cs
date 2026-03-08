namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Interface for creating reusable async custom validators.
/// </summary>
/// <typeparam name="T">The type of value to validate.</typeparam>
public interface IAsyncCustomValidator<in T>
{
    /// <summary>
    /// Validates the given value asynchronously.
    /// </summary>
    Task<bool> IsValidAsync(T value);

    /// <summary>
    /// Returns the failure message when validation fails.
    /// </summary>
    string GetFailureMessage(T value);
}
