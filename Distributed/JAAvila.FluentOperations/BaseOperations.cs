namespace JAAvila.FluentOperations;

/// <summary>
/// Provides fault-tolerant execution helpers used by <see cref="ExecutionEngine{T,TS}"/> to
/// run validators without propagating unexpected exceptions. Any exception thrown inside the
/// delegate is caught and treated as a failed validation (<c>false</c>).
/// </summary>
internal class BaseOperations
{
    /// <summary>
    /// Executes <paramref name="action"/> and returns its result.
    /// Returns <c>false</c> if an exception is thrown.
    /// </summary>
    /// <param name="action">The synchronous validation delegate to execute.</param>
    /// <returns><c>true</c> if the action completes successfully and returns <c>true</c>; otherwise <c>false</c>.</returns>
    public static bool SafeExecute(Func<bool> action)
    {
        try
        {
            return action();
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Executes <paramref name="action"/> asynchronously and returns its result.
    /// Returns <c>false</c> if an exception is thrown.
    /// </summary>
    /// <param name="action">The asynchronous validation delegate to execute.</param>
    /// <returns><c>true</c> if the task completes successfully and returns <c>true</c>; otherwise <c>false</c>.</returns>
    public static async Task<bool> SafeExecuteAsync(Func<Task<bool>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception)
        {
            return false;
        }
    }
}
