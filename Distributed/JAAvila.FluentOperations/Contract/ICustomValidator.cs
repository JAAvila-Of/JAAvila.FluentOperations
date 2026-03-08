namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Interface for creating reusable custom validators.
/// Implement this interface to encapsulate complex validation logic
/// that can be used across multiple Blueprints.
/// </summary>
/// <typeparam name="T">The type of value to validate.</typeparam>
public interface ICustomValidator<in T>
{
    /// <summary>
    /// Validates the given value.
    /// </summary>
    /// <returns>true if valid, false otherwise.</returns>
    bool IsValid(T value);

    /// <summary>
    /// Returns the failure message when validation fails.
    /// </summary>
    string GetFailureMessage(T value);
}
