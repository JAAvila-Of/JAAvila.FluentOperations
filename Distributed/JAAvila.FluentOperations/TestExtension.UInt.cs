using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Manager;

namespace JAAvila.FluentOperations;

public static partial class TestExtension
{
    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="uint"/> value.
    /// </summary>
    /// <param name="value">The uint value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="UIntOperationsManager"/> for chaining uint-specific assertions.</returns>
    /// <example>
    /// <code>
    /// uint count = 42u;
    /// count.Test().BePositive().BeLessThan(100u);
    /// </code>
    /// </example>
    [Pure]
    public static UIntOperationsManager Test(
        this uint value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new UIntOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="uint"/> value.
    /// </summary>
    /// <param name="value">The nullable uint value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableUIntOperationsManager"/> for chaining nullable uint-specific assertions.</returns>
    /// <example>
    /// <code>
    /// uint? count = null;
    /// count.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableUIntOperationsManager Test(
        this uint? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableUIntOperationsManager(value, callerName);
    }
}
