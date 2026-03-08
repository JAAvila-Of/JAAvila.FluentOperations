using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Manager;

namespace JAAvila.FluentOperations;

public static partial class TestExtension
{
    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="Dictionary{TKey, TValue}"/> value.
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of dictionary values.</typeparam>
    /// <param name="value">The dictionary to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="DictionaryOperationsManager{TKey, TValue}"/> for chaining dictionary-specific assertions.</returns>
    /// <example>
    /// <code>
    /// var settings = new Dictionary&lt;string, int&gt; { ["timeout"] = 30 };
    /// settings.Test().ContainKey("timeout");
    /// </code>
    /// </example>
    [Pure]
    public static DictionaryOperationsManager<TKey, TValue> Test<TKey, TValue>(
        this Dictionary<TKey, TValue> value,
        [CallerArgumentExpression("value")] string callerName = ""
    )
        where TKey : notnull => new(value, callerName);

    /// <summary>
    /// Begins a fluent assertion chain for the specified array value.
    /// </summary>
    /// <typeparam name="T">The type of array elements.</typeparam>
    /// <param name="value">The array to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>An <see cref="ArrayOperationsManager{T}"/> for chaining array-specific assertions.</returns>
    /// <example>
    /// <code>
    /// int[] numbers = { 1, 2, 3 };
    /// numbers.Test().HaveLength(3).NotBeEmpty();
    /// </code>
    /// </example>
    [Pure]
    public static ArrayOperationsManager<T> Test<T>(
        this T[] value,
        [CallerArgumentExpression("value")] string callerName = ""
    ) => new(value, callerName);

    /// <summary>
    /// Begins a fluent assertion chain for the specified <see cref="IEnumerable{T}"/> value.
    /// </summary>
    /// <typeparam name="T">The type of collection elements.</typeparam>
    /// <param name="value">The enumerable collection to test.</param>
    /// <param name="callerName">
    /// Automatically captured expression name of the variable being tested.
    /// Used in failure messages for contextual reporting.
    /// </param>
    /// <returns>A <see cref="CollectionOperationsManager{T}"/> for chaining collection-specific assertions.</returns>
    /// <example>
    /// <code>
    /// var items = new[] { 1, 2, 3 }.AsEnumerable();
    /// items.Test().HaveCount(3).NotBeEmpty();
    /// </code>
    /// </example>
    [Pure]
    public static CollectionOperationsManager<T> Test<T>(
        this IEnumerable<T> value,
        [CallerArgumentExpression("value")] string callerName = ""
    ) => new(value, callerName);
}
