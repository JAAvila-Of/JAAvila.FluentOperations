using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Manager;

namespace JAAvila.FluentOperations;

public static partial class TestExtension
{
    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="ulong"/> value.
    /// </summary>
    /// <param name="value">The ulong value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="ULongOperationsManager"/> for chaining ulong-specific assertions.</returns>
    /// <example>
    /// <code>
    /// ulong count = 42UL;
    /// count.Test().BePositive().BeLessThan(100UL);
    /// </code>
    /// </example>
    [Pure]
    public static ULongOperationsManager Test(
        this ulong value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new ULongOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="ulong"/> value.
    /// </summary>
    /// <param name="value">The nullable ulong value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableULongOperationsManager"/> for chaining nullable ulong-specific assertions.</returns>
    /// <example>
    /// <code>
    /// ulong? count = null;
    /// count.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableULongOperationsManager Test(
        this ulong? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableULongOperationsManager(value, callerName);
    }
}
