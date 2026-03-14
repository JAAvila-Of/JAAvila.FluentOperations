using System.Diagnostics;

namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Captures execution statistics from invoking an Action or Func delegate.
/// Includes timing, memory allocation, and exception information.
/// </summary>
public sealed class ActionStats
{
    /// <summary>
    /// The elapsed time of the delegate execution.
    /// </summary>
    public TimeSpan ElapsedTime { get; init; }

    /// <summary>
    /// Convenience property: total elapsed milliseconds.
    /// </summary>
    public double ElapsedMilliseconds => ElapsedTime.TotalMilliseconds;

    /// <summary>
    /// Whether the delegate completed without throwing an exception.
    /// </summary>
    public bool Succeeded { get; init; }

    /// <summary>
    /// The exception captured during execution, or null if the delegate succeeded.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// The type of the captured exception, or null if no exception was thrown.
    /// </summary>
    public Type? ExceptionType => Exception?.GetType();

    /// <summary>
    /// Approximate memory allocation delta in bytes (GC.GetTotalMemory after - before).
    /// May be negative if a GC occurred during execution.
    /// </summary>
    public long MemoryDelta { get; init; }

    /// <summary>
    /// The return value of the delegate, if it was a Func{T} or Func{Task{T}}.
    /// Null for Action and Func{Task} delegates, or when the delegate threw.
    /// </summary>
    public object? ReturnValue { get; init; }

    /// <summary>
    /// Executes a synchronous Action and captures execution statistics.
    /// </summary>
    internal static ActionStats Capture(Action action)
    {
        var memBefore = GC.GetTotalMemory(forceFullCollection: false);
        var sw = Stopwatch.StartNew();
        Exception? caught = null;

        try
        {
            action();
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        sw.Stop();
        var memAfter = GC.GetTotalMemory(forceFullCollection: false);

        return new ActionStats
        {
            ElapsedTime = sw.Elapsed,
            Succeeded = caught is null,
            Exception = caught,
            MemoryDelta = memAfter - memBefore,
            ReturnValue = null
        };
    }

    /// <summary>
    /// Executes a synchronous Func{T} and captures execution statistics.
    /// </summary>
    internal static ActionStats Capture<T>(Func<T> func)
    {
        var memBefore = GC.GetTotalMemory(forceFullCollection: false);
        var sw = Stopwatch.StartNew();
        Exception? caught = null;
        object? result = null;

        try
        {
            result = func();
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        sw.Stop();
        var memAfter = GC.GetTotalMemory(forceFullCollection: false);

        return new ActionStats
        {
            ElapsedTime = sw.Elapsed,
            Succeeded = caught is null,
            Exception = caught,
            MemoryDelta = memAfter - memBefore,
            ReturnValue = caught is null ? result : null
        };
    }

    /// <summary>
    /// Executes an asynchronous Func{Task} and captures execution statistics.
    /// </summary>
    internal static async Task<ActionStats> CaptureAsync(Func<Task> asyncAction)
    {
        var memBefore = GC.GetTotalMemory(forceFullCollection: false);
        var sw = Stopwatch.StartNew();
        Exception? caught = null;

        try
        {
            await asyncAction();
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        sw.Stop();
        var memAfter = GC.GetTotalMemory(forceFullCollection: false);

        return new ActionStats
        {
            ElapsedTime = sw.Elapsed,
            Succeeded = caught is null,
            Exception = caught,
            MemoryDelta = memAfter - memBefore,
            ReturnValue = null
        };
    }

    /// <summary>
    /// Executes an asynchronous Func{Task{T}} and captures execution statistics.
    /// </summary>
    internal static async Task<ActionStats> CaptureAsync<T>(Func<Task<T>> asyncFunc)
    {
        var memBefore = GC.GetTotalMemory(forceFullCollection: false);
        var sw = Stopwatch.StartNew();
        Exception? caught = null;
        object? result = null;

        try
        {
            result = await asyncFunc();
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        sw.Stop();
        var memAfter = GC.GetTotalMemory(forceFullCollection: false);

        return new ActionStats
        {
            ElapsedTime = sw.Elapsed,
            Succeeded = caught is null,
            Exception = caught,
            MemoryDelta = memAfter - memBefore,
            ReturnValue = caught is null ? result : null
        };
    }
}
