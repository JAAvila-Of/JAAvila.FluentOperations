using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Manager;

namespace JAAvila.FluentOperations;

public static partial class TestExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IntegerOperationsManager"/> class,
    /// allowing fluent test operations and validations on the integer value.
    /// </summary>
    /// <param name="value">The integer value on which the test operations will be performed.</param>
    /// <param name="callerName">The name of the parameter being tested, used for contextual error reporting.
    /// Defaults to the parameter name if not explicitly provided.</param>
    /// <returns>An instance of <see cref="IntegerOperationsManager"/> to enable chainable integer test operations.</returns>
    [Pure]
    public static IntegerOperationsManager Test(
        this int value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new IntegerOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="int"/> value.
    /// </summary>
    /// <param name="value">The nullable int value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableIntegerOperationsManager"/> for chaining nullable int-specific assertions.</returns>
    /// <example>
    /// <code>
    /// int? count = 5;
    /// count.Test().HaveValue().BePositive();
    /// </code>
    /// </example>
    [Pure]
    public static NullableIntegerOperationsManager Test(
        this int? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableIntegerOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="long"/> value.
    /// </summary>
    /// <param name="value">The long value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="LongOperationsManager"/> for chaining long-specific assertions.</returns>
    /// <example>
    /// <code>
    /// long bigNumber = 1_000_000L;
    /// bigNumber.Test().BePositive();
    /// </code>
    /// </example>
    [Pure]
    public static LongOperationsManager Test(
        this long value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new LongOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="long"/> value.
    /// </summary>
    /// <param name="value">The nullable long value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableLongOperationsManager"/> for chaining nullable long-specific assertions.</returns>
    /// <example>
    /// <code>
    /// long? fileSize = null;
    /// fileSize.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableLongOperationsManager Test(
        this long? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableLongOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="decimal"/> value.
    /// </summary>
    /// <param name="value">The decimal value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="DecimalOperationsManager"/> for chaining decimal-specific assertions.</returns>
    /// <example>
    /// <code>
    /// decimal price = 9.99m;
    /// price.Test().BePositive().BeLessThan(100m);
    /// </code>
    /// </example>
    [Pure]
    public static DecimalOperationsManager Test(
        this decimal value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new DecimalOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="decimal"/> value.
    /// </summary>
    /// <param name="value">The nullable decimal value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableDecimalOperationsManager"/> for chaining nullable decimal-specific assertions.</returns>
    /// <example>
    /// <code>
    /// decimal? discount = null;
    /// discount.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableDecimalOperationsManager Test(
        this decimal? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableDecimalOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="double"/> value.
    /// </summary>
    /// <param name="value">The double value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="DoubleOperationsManager"/> for chaining double-specific assertions.</returns>
    /// <example>
    /// <code>
    /// double ratio = 0.75;
    /// ratio.Test().BeInRange(0.0, 1.0);
    /// </code>
    /// </example>
    [Pure]
    public static DoubleOperationsManager Test(
        this double value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new DoubleOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="double"/> value.
    /// </summary>
    /// <param name="value">The nullable double value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableDoubleOperationsManager"/> for chaining nullable double-specific assertions.</returns>
    /// <example>
    /// <code>
    /// double? measurement = null;
    /// measurement.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableDoubleOperationsManager Test(
        this double? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableDoubleOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="float"/> value.
    /// </summary>
    /// <param name="value">The float value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="FloatOperationsManager"/> for chaining float-specific assertions.</returns>
    /// <example>
    /// <code>
    /// float temperature = 36.6f;
    /// temperature.Test().BePositive().BeLessThan(100f);
    /// </code>
    /// </example>
    [Pure]
    public static FloatOperationsManager Test(
        this float value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new FloatOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="float"/> value.
    /// </summary>
    /// <param name="value">The nullable float value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableFloatOperationsManager"/> for chaining nullable float-specific assertions.</returns>
    /// <example>
    /// <code>
    /// float? weight = null;
    /// weight.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableFloatOperationsManager Test(
        this float? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableFloatOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="byte"/> value.
    /// </summary>
    /// <param name="value">The byte value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="ByteOperationsManager"/> for chaining byte-specific assertions.</returns>
    /// <example>
    /// <code>
    /// byte age = 25;
    /// age.Test().BePositive().BeLessThan(128);
    /// </code>
    /// </example>
    [Pure]
    public static ByteOperationsManager Test(
        this byte value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new ByteOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="byte"/> value.
    /// </summary>
    /// <param name="value">The nullable byte value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableByteOperationsManager"/> for chaining nullable byte-specific assertions.</returns>
    /// <example>
    /// <code>
    /// byte? channel = null;
    /// channel.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableByteOperationsManager Test(
        this byte? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableByteOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="sbyte"/> value.
    /// </summary>
    /// <param name="value">The sbyte value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="SByteOperationsManager"/> for chaining sbyte-specific assertions.</returns>
    /// <example>
    /// <code>
    /// sbyte temperature = -10;
    /// temperature.Test().BeNegative().BeLessThan(0);
    /// </code>
    /// </example>
    [Pure]
    public static SByteOperationsManager Test(
        this sbyte value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new SByteOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="sbyte"/> value.
    /// </summary>
    /// <param name="value">The nullable sbyte value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableSByteOperationsManager"/> for chaining nullable sbyte-specific assertions.</returns>
    /// <example>
    /// <code>
    /// sbyte? offset = null;
    /// offset.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableSByteOperationsManager Test(
        this sbyte? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableSByteOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="char"/> value.
    /// </summary>
    /// <param name="value">The char value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="CharOperationsManager"/> for chaining char-specific assertions.</returns>
    /// <example>
    /// <code>
    /// char letter = 'A';
    /// letter.Test().BeUpperCase().BeLetter();
    /// </code>
    /// </example>
    [Pure]
    public static CharOperationsManager Test(
        this char value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new CharOperationsManager(value, callerName);
    }

    /// <summary>
    /// Begins a fluent assertion chain for the specified nullable <see cref="char"/> value.
    /// </summary>
    /// <param name="value">The nullable char value to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="NullableCharOperationsManager"/> for chaining nullable char-specific assertions.</returns>
    /// <example>
    /// <code>
    /// char? initial = null;
    /// initial.Test().NotHaveValue();
    /// </code>
    /// </example>
    [Pure]
    public static NullableCharOperationsManager Test(
        this char? value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
    {
        return new NullableCharOperationsManager(value, callerName);
    }
}
