using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Manager;

namespace JAAvila.FluentOperations;

public static partial class TestExtension
{
    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="ushort"/> value.
    /// </summary>
    /// <param name="value">The ushort value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="UShortOperationsManager"/> for chaining ushort-specific assertions.</returns>
    /// <example>
    /// <code>
    /// ushort port = 8080;
    /// port.Test().BePositive().BeLessThan(65535);
    /// </code>
    /// </example>
    [Pure]
    public static UShortOperationsManager Test(
        this ushort value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new UShortOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="ushort"/> value.
    /// </summary>
    /// <param name="value">The nullable ushort value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableUShortOperationsManager"/> for chaining nullable ushort-specific assertions.</returns>
    /// <example>
    /// <code>
    /// ushort? port = null;
    /// port.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableUShortOperationsManager Test(
        this ushort? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableUShortOperationsManager(value, callerName);
    }
}
