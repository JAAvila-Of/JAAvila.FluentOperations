using JAAvila.FluentOperations.Exceptions;

namespace JAAvila.FluentOperations.Utils;

/// <summary>
/// Guards entry to individual operation methods in manager classes, ensuring the operation
/// is valid and the transactional context permits execution.
/// </summary>
internal static class OperationUtils
{
    /// <summary>
    /// Verifies that <paramref name="value"/> is a defined member of enum <typeparamref name="T"/>
    /// and that the current <see cref="TransactionalOperations"/> scope (if any) has not been
    /// interrupted. Returns <c>false</c> when execution should be skipped without error;
    /// throws <see cref="Exceptions.FluentOperationsException"/> when the operation is unsupported.
    /// </summary>
    /// <typeparam name="T">The operation enum type.</typeparam>
    /// <param name="value">The operation enum value to validate.</param>
    /// <returns><c>true</c> if the operation is allowed; <c>false</c> if it should be skipped.</returns>
    public static bool CheckOperationAllowed<T>(T value)
        where T : struct, Enum
    {
        if (!TransactionalOperations.CanContinue())
        {
            return false;
        }

        if (!Enum.IsDefined(value))
        {
            throw new FluentOperationsException(
                "The operation is not supported for the validator type."
            );
        }

        return true;
    }
}
