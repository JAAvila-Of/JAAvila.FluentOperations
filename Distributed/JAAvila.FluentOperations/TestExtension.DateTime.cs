using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Manager;

namespace JAAvila.FluentOperations;

public static partial class TestExtension
{
    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="value">The DateTime value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="DateTimeOperationsManager"/> for chaining DateTime-specific assertions.</returns>
    /// <example>
    /// <code>
    /// DateTime now = DateTime.Now;
    /// now.Test().BeAfter(DateTime.MinValue);
    /// </code>
    /// </example>
    [Pure]
    public static DateTimeOperationsManager Test(
        this DateTime value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new DateTimeOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="value">The nullable DateTime value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableDateTimeOperationsManager"/> for chaining nullable DateTime-specific assertions.</returns>
    /// <example>
    /// <code>
    /// DateTime? expiry = null;
    /// expiry.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableDateTimeOperationsManager Test(
        this DateTime? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableDateTimeOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="DateOnly"/> value.
    /// </summary>
    /// <param name="value">The DateOnly value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="DateOnlyOperationsManager"/> for chaining DateOnly-specific assertions.</returns>
    /// <example>
    /// <code>
    /// DateOnly birthday = new DateOnly(1990, 6, 15);
    /// birthday.Test().BeBefore(DateOnly.FromDateTime(DateTime.Today));
    /// </code>
    /// </example>
    [Pure]
    public static DateOnlyOperationsManager Test(
        this DateOnly value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new DateOnlyOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="DateOnly"/> value.
    /// </summary>
    /// <param name="value">The nullable DateOnly value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableDateOnlyOperationsManager"/> for chaining nullable DateOnly-specific assertions.</returns>
    /// <example>
    /// <code>
    /// DateOnly? endDate = null;
    /// endDate.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableDateOnlyOperationsManager Test(
        this DateOnly? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableDateOnlyOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="TimeSpan"/> value.
    /// </summary>
    /// <param name="value">The TimeSpan value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="TimeSpanOperationsManager"/> for chaining TimeSpan-specific assertions.</returns>
    /// <example>
    /// <code>
    /// TimeSpan duration = TimeSpan.FromMinutes(30);
    /// duration.Test().BePositive();
    /// </code>
    /// </example>
    [Pure]
    public static TimeSpanOperationsManager Test(
        this TimeSpan value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new TimeSpanOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="TimeSpan"/> value.
    /// </summary>
    /// <param name="value">The nullable TimeSpan value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableTimeSpanOperationsManager"/> for chaining nullable TimeSpan-specific assertions.</returns>
    /// <example>
    /// <code>
    /// TimeSpan? timeout = null;
    /// timeout.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableTimeSpanOperationsManager Test(
        this TimeSpan? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableTimeSpanOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="TimeOnly"/> value.
    /// </summary>
    /// <param name="value">The TimeOnly value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="TimeOnlyOperationsManager"/> for chaining TimeOnly-specific assertions.</returns>
    /// <example>
    /// <code>
    /// TimeOnly startTime = new TimeOnly(9, 0);
    /// startTime.Test().BeBefore(new TimeOnly(17, 0));
    /// </code>
    /// </example>
    [Pure]
    public static TimeOnlyOperationsManager Test(
        this TimeOnly value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new TimeOnlyOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="TimeOnly"/> value.
    /// </summary>
    /// <param name="value">The nullable TimeOnly value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableTimeOnlyOperationsManager"/> for chaining nullable TimeOnly-specific assertions.</returns>
    /// <example>
    /// <code>
    /// TimeOnly? meetingTime = null;
    /// meetingTime.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableTimeOnlyOperationsManager Test(
        this TimeOnly? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableTimeOnlyOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="value">The DateTimeOffset value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="DateTimeOffsetOperationsManager"/> for chaining DateTimeOffset-specific assertions.</returns>
    /// <example>
    /// <code>
    /// DateTimeOffset timestamp = DateTimeOffset.UtcNow;
    /// timestamp.Test().BeAfter(DateTimeOffset.MinValue);
    /// </code>
    /// </example>
    [Pure]
    public static DateTimeOffsetOperationsManager Test(
        this DateTimeOffset value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new DateTimeOffsetOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="value">The nullable DateTimeOffset value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableDateTimeOffsetOperationsManager"/> for chaining nullable DateTimeOffset-specific assertions.</returns>
    /// <example>
    /// <code>
    /// DateTimeOffset? scheduledAt = null;
    /// scheduledAt.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableDateTimeOffsetOperationsManager Test(
        this DateTimeOffset? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableDateTimeOffsetOperationsManager(value, callerName);
    }
}
