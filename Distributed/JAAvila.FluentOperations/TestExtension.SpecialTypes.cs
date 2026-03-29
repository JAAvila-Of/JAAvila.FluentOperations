using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Manager;

namespace JAAvila.FluentOperations;

public static partial class TestExtension
{
    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="Guid"/> value.
    /// </summary>
    /// <param name="value">The Guid value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="GuidOperationsManager"/> for chaining Guid-specific assertions.</returns>
    /// <example>
    /// <code>
    /// Guid id = Guid.NewGuid();
    /// id.Test().NotBeEmpty();
    /// </code>
    /// </example>
    [Pure]
    public static GuidOperationsManager Test(
        this Guid value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new GuidOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="Guid"/> value.
    /// </summary>
    /// <param name="value">The nullable Guid value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableGuidOperationsManager"/> for chaining nullable Guid-specific assertions.</returns>
    /// <example>
    /// <code>
    /// Guid? correlationId = null;
    /// correlationId.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableGuidOperationsManager Test(
        this Guid? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableGuidOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified enum value.
    /// </summary>
    /// <typeparam name="T">The enum type to test. Must derive from <see cref="Enum"/>.</typeparam>
    /// <param name="value">The enum value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>An <see cref="EnumOperationsManager{T}"/> for chaining enum-specific assertions.</returns>
    /// <remarks>
    /// Use <c>TestEnum&lt;T&gt;()</c> Instead of <c>Test()</c> for enum values to avoid ambiguity
    /// with the <c>object?</c> Overload.
    /// </remarks>
    /// <example>
    /// <code>
    /// DayOfWeek day = DayOfWeek.Monday;
    /// day.TestEnum().BeDefined();
    /// </code>
    /// </example>
    [Pure]
    public static EnumOperationsManager<T> TestEnum<T>(
        this T value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
        where T : Enum
    {
        return new EnumOperationsManager<T>(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable enum value.
    /// </summary>
    /// <typeparam name="T">The enum type to test. Must be a struct and derive from <see cref="Enum"/>.</typeparam>
    /// <param name="value">The nullable enum value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableEnumOperationsManager{T}"/> for chaining nullable enum-specific assertions.</returns>
    [Pure]
    public static NullableEnumOperationsManager<T> TestEnum<T>(
        this T? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
        where T : struct, Enum
    {
        return new NullableEnumOperationsManager<T>(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="Uri"/> value.
    /// </summary>
    /// <param name="value">The Uri value to test. Can be null.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="UriOperationsManager"/> for chaining Uri-specific assertions.</returns>
    /// <example>
    /// <code>
    /// var uri = new Uri("https://example.com");
    /// uri.Test().HaveScheme("https").BeAbsolute();
    /// </code>
    /// </example>
    [Pure]
    public static UriOperationsManager Test(
        this Uri? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new UriOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="Type"/> value.
    /// </summary>
    /// <param name="value">The Type value to test. Can be null.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="TypeOperationsManager"/> for chaining Type-specific assertions.</returns>
    /// <example>
    /// <code>
    /// typeof(MyService).Test().BeClass().BeSealed().BePublic();
    /// </code>
    /// </example>
    [Pure]
    public static TypeOperationsManager Test(
        this Type? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new TypeOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="Assembly"/> value.
    /// </summary>
    /// <param name="value">The Assembly value to test. Can be null.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>An <see cref="AssemblyOperationsManager"/> for chaining Assembly-specific assertions.</returns>
    /// <example>
    /// <code>
    /// typeof(MyService).Assembly.Test()
    ///     .ContainType&lt;MyService&gt;()
    ///     .NotReferenceAssembly("Newtonsoft.Json");
    /// </code>
    /// </example>
    [Pure]
    public static AssemblyOperationsManager Test(
        this Assembly? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new AssemblyOperationsManager(value, callerName);
    }
}
