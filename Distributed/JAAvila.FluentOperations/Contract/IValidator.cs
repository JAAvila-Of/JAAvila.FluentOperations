namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Represents a contract for implementing validators that perform validation operations.
/// </summary>
public interface IValidator
{
    /// <summary>
    /// A formatted string describing the expected value or condition for this validation.
    /// Typically used when building the failure message.
    /// </summary>
    string Expected { get; }

    /// <summary>
    /// A composite-format template string that describes why the validation failed.
    /// Set by the validator's <see cref="Validate"/> or <see cref="ValidateAsync"/> method when the check does not pass.
    /// </summary>
    string ResultValidation { get; set; }

    /// <summary>
    /// A stable key identifying this validator's failure message, used for localization lookup.
    /// Format: <c>"TypePrefix.OperationName"</c> (e.g., <c>"String.BeEmail"</c>).
    /// </summary>
    string MessageKey { get; }

    /// <summary>
    /// Validates the associated value based on the implemented validation logic.
    /// </summary>
    /// <returns>
    /// A boolean value indicating whether the validation succeeded.
    /// Returns <c>true</c> if the validation passes; otherwise, <c>false</c>.
    /// </returns>
    bool Validate();

    /// <summary>
    /// Asynchronously validates the associated value based on the implemented validation logic.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a boolean value indicating whether the validation succeeded.
    /// Returns <c>true</c> if the validation passes; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> ValidateAsync();
}
