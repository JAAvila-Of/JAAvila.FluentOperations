using System.Diagnostics;

namespace JAAvila.FluentOperations.Utils;

/// <summary>
/// Provides utilities for inspecting the current call stack, used to detect which types
/// are present in the execution chain (e.g., for test-framework auto-detection).
/// </summary>
internal static class StackUtils
{
    /// <summary>
    /// Captures the current call stack and returns the declaring types of all stack frames,
    /// filtering out frames with no associated type.
    /// </summary>
    /// <returns>An enumerable of non-null <see cref="Type"/> values from the current stack.</returns>
    public static IEnumerable<Type> GetTypesFromStack()
    {
        var stackTrace = new StackTrace();
        var frames = stackTrace.GetFrames();
        return frames.Select(x => x.GetMethod()?.DeclaringType).Where(x => x != null)!;
    }
}
