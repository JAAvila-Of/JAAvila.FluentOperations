using System.Diagnostics.Contracts;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

/// <summary>
/// Provides extension methods to capture execution statistics from Action and Func delegates.
/// </summary>
public static class StatsExtension
{
    /// <summary>
    /// Executes the synchronous action and captures execution statistics.
    /// </summary>
    /// <param name="action">The action to execute and measure.</param>
    /// <returns>An <see cref="ActionStats"/> containing the captured statistics.</returns>
    [Pure]
    public static ActionStats Stats(this Action action)
    {
        return ActionStats.Capture(action);
    }

    /// <summary>
    /// Executes the synchronous function and captures execution statistics.
    /// </summary>
    /// <typeparam name="T">The return type of the function.</typeparam>
    /// <param name="func">The function to execute and measure.</param>
    /// <returns>An <see cref="ActionStats"/> containing the captured statistics.</returns>
    [Pure]
    public static ActionStats Stats<T>(this Func<T> func)
    {
        return ActionStats.Capture(func);
    }

    /// <summary>
    /// Executes the asynchronous action and captures execution statistics.
    /// </summary>
    /// <param name="asyncAction">The async action to execute and measure.</param>
    /// <returns>A task containing an <see cref="ActionStats"/> with the captured statistics.</returns>
    [Pure]
    public static Task<ActionStats> StatsAsync(this Func<Task> asyncAction)
    {
        return ActionStats.CaptureAsync(asyncAction);
    }

    /// <summary>
    /// Executes the asynchronous function and captures execution statistics.
    /// </summary>
    /// <typeparam name="T">The return type of the async function.</typeparam>
    /// <param name="asyncFunc">The async function to execute and measure.</param>
    /// <returns>A task containing an <see cref="ActionStats"/> with the captured statistics.</returns>
    [Pure]
    public static Task<ActionStats> StatsAsync<T>(this Func<Task<T>> asyncFunc)
    {
        return ActionStats.CaptureAsync(asyncFunc);
    }
}
