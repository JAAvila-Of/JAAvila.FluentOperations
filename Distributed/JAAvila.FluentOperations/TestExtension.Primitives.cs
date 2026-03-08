using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Manager;

namespace JAAvila.FluentOperations;

public static partial class TestExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StringOperationsManager"/> class,
    /// allowing fluent test operations and validations on the string value.
    /// </summary>
    /// <param name="value">The string value on which the test operations will be performed. Can be null.</param>
    /// <param name="callerName">The name of the parameter being tested, used for contextual error reporting.
    /// Defaults to the parameter name if not explicitly provided.</param>
    /// <returns>An instance of <see cref="StringOperationsManager"/> to enable chainable string test operations.</returns>
    [Pure]
    public static StringOperationsManager Test(
        this string? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new StringOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="bool"/> value.
    /// </summary>
    /// <param name="value">The nullable bool value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableBooleanOperationsManager"/> for chaining nullable bool-specific assertions.</returns>
    /// <example>
    /// <code>
    /// bool? isActive = null;
    /// isActive.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableBooleanOperationsManager Test(
        this bool? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableBooleanOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="bool"/> value.
    /// </summary>
    /// <param name="value">The bool value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="BooleanOperationsManager"/> for chaining bool-specific assertions.</returns>
    /// <example>
    /// <code>
    /// bool isEnabled = true;
    /// isEnabled.Test().BeTrue();
    /// </code>
    /// </example>
    [Pure]
    public static BooleanOperationsManager Test(
        this bool value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new BooleanOperationsManager(value, callerName);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectOperationsManager"/> class,
    /// allowing fluent test operations and validations on the object value.
    /// </summary>
    /// <param name="value">The object value on which the test operations will be performed. Can be null.</param>
    /// <param name="callerName">The name of the parameter being tested, used for contextual error reporting.
    /// Defaults to the parameter name if not explicitly provided.</param>
    /// <returns>An instance of <see cref="ObjectOperationsManager"/> to enable chainable object test operations.</returns>
    [Pure]
    public static ObjectOperationsManager Test(
        this object? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new ObjectOperationsManager(value, callerName);
    }
}
