using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

namespace JAAvila.FluentOperations.Comparators;

/// <summary>
/// Performs recursive deep equality comparison of objects by their public properties.
/// Handles cycles, configurable recursion depth, excluded properties, and collection comparison.
/// </summary>
public class ObjectComparator : IComparator<object?>
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();

    ComparisonResult IComparator<object?>.Compare(object? actual, object? expected) =>
        DeepCompare(actual, expected, ComparisonOptions.Default);

    ComparisonResult IComparator<object?>.Compare(
        object? actual,
        object? expected,
        ComparisonOptions options
    ) => DeepCompare(actual, expected, options);

    /// <summary>
    /// Performs a deep structural comparison between two objects.
    /// </summary>
    /// <param name="actual">The actual object.</param>
    /// <param name="expected">The expected object.</param>
    /// <param name="options">Optional comparison options. Defaults to ComparisonOptions.Default.</param>
    public static ComparisonResult DeepCompare(
        object? actual,
        object? expected,
        ComparisonOptions? options = null
    )
    {
        var opts = options ?? ComparisonOptions.Default;
        var differences = new List<string>();
        var visited = new HashSet<object>(ReferenceEqualityComparer.Instance);

        CompareRecursive(actual, expected, string.Empty, 0, opts, visited, differences);

        if (differences.Count == 0)
        {
            return ComparisonResult.Equal;
        }

        var sb = new StringBuilder();

        for (var i = 0; i < differences.Count; i++)
        {
            if (i > 0)
            {
                sb.AppendLine();
            }

            sb.Append(differences[i]);
        }

        return ComparisonResult.NotEqual(sb.ToString());
    }

    private static void CompareRecursive(
        object? actual,
        object? expected,
        string path,
        int depth,
        ComparisonOptions options,
        HashSet<object> visited,
        List<string> differences
    )
    {
        if (differences.Count >= options.MaxDifferencesReported)
        {
            return;
        }

        if (actual is null && expected is null)
        {
            return;
        }

        if (actual is null)
        {
            var label = string.IsNullOrEmpty(path) ? "<root>" : $"Property '{path}'";
            differences.Add($"{label}: expected '{FormatValue(expected)}' but found <null>");
            return;
        }

        if (expected is null)
        {
            var label = string.IsNullOrEmpty(path) ? "<root>" : $"Property '{path}'";
            differences.Add($"{label}: expected <null> but found '{FormatValue(actual)}'");
            return;
        }

        // Same reference: equal
        if (ReferenceEquals(actual, expected))
        {
            return;
        }

        // Cycle detection: if actual was already visited, skip to avoid infinite recursion
        if (!actual.GetType().IsValueType && visited.Contains(actual))
        {
            return;
        }

        var actualType = actual.GetType();

        // Primitive types: use Equals
        if (IsPrimitive(actualType))
        {
            if (!actual.Equals(expected))
            {
                var label = string.IsNullOrEmpty(path) ? "<root>" : $"Property '{path}'";
                differences.Add(
                    $"{label}: expected '{FormatValue(expected)}' but found '{FormatValue(actual)}'"
                );
            }

            return;
        }

        // Collection (non-string)
        if (IsCollection(actual))
        {
            if (!actual.GetType().IsValueType)
            {
                visited.Add(actual);
            }

            CompareCollections(actual, expected, path, depth, options, visited, differences);
            return;
        }

        // Max depth guard
        if (depth > options.MaxRecursionDepth)
        {
            if (differences.Count < options.MaxDifferencesReported)
            {
                differences.Add(
                    $"Property '{path}': max recursion depth ({options.MaxRecursionDepth}) reached, comparison skipped"
                );
            }

            return;
        }

        // Complex object: recurse by public properties
        if (!actual.GetType().IsValueType)
        {
            visited.Add(actual);
        }

        var properties = GetPublicProperties(actualType);

        foreach (var prop in properties)
        {
            if (differences.Count >= options.MaxDifferencesReported)
            {
                return;
            }

            var propName = prop.Name;

            // Check if a property is excluded (by simple name or full path)
            if (options.ExcludedProperties.Contains(propName))
            {
                continue;
            }

            var childPath = string.IsNullOrEmpty(path) ? propName : $"{path}.{propName}";

            if (options.ExcludedProperties.Contains(childPath))
            {
                continue;
            }

            object? actualVal;
            object? expectedVal = null;

            try
            {
                actualVal = prop.GetValue(actual);
            }
            catch
            {
                continue;
            }

            // If expected is a different type, try to get the same property
            var expectedType = expected.GetType();
            var expectedProp = expectedType.GetProperty(
                propName,
                BindingFlags.Public | BindingFlags.Instance
            );

            if (expectedProp != null)
            {
                try
                {
                    expectedVal = expectedProp.GetValue(expected);
                }
                catch
                { /* ignore */
                }
            }

            CompareRecursive(
                actualVal,
                expectedVal,
                childPath,
                depth + 1,
                options,
                visited,
                differences
            );
        }
    }

    private static void CompareCollections(
        object actual,
        object? expected,
        string path,
        int depth,
        ComparisonOptions options,
        HashSet<object> visited,
        List<string> differences
    )
    {
        if (differences.Count >= options.MaxDifferencesReported)
        {
            return;
        }

        var actualList = ((IEnumerable)actual).Cast<object?>().ToList();

        if (expected is not IEnumerable expectedEnumerable)
        {
            var label = string.IsNullOrEmpty(path) ? "<root>" : $"Collection at '{path}'";
            differences.Add($"{label}: expected value is not a collection");
            return;
        }

        var expectedList = expectedEnumerable.Cast<object?>().ToList();

        if (actualList.Count != expectedList.Count)
        {
            var label = string.IsNullOrEmpty(path) ? "<root>" : $"Collection at '{path}'";
            differences.Add(
                $"{label}: expected {expectedList.Count} items but found {actualList.Count}"
            );
            return;
        }

        var count = Math.Min(actualList.Count, expectedList.Count);

        for (var i = 0; i < count; i++)
        {
            if (differences.Count >= options.MaxDifferencesReported)
            {
                return;
            }

            var childPath = string.IsNullOrEmpty(path) ? $"[{i}]" : $"{path}[{i}]";
            CompareRecursive(
                actualList[i],
                expectedList[i],
                childPath,
                depth + 1,
                options,
                visited,
                differences
            );
        }
    }

    private static bool IsPrimitive(Type type)
    {
        var underlying = Nullable.GetUnderlyingType(type) ?? type;
        return underlying.IsPrimitive
            || underlying == typeof(string)
            || underlying == typeof(decimal)
            || underlying == typeof(DateTime)
            || underlying == typeof(DateTimeOffset)
            || underlying == typeof(DateOnly)
            || underlying == typeof(TimeOnly)
            || underlying == typeof(TimeSpan)
            || underlying == typeof(Guid)
            || underlying.IsEnum;
    }

    private static bool IsCollection(object obj) => obj is IEnumerable and not string;

    private static PropertyInfo[] GetPublicProperties(Type type)
    {
        return PropertyCache.GetOrAdd(
            type,
            t =>
                t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                    .ToArray()
        );
    }

    private static string FormatValue(object? value) => value?.ToString() ?? "<null>";
}
