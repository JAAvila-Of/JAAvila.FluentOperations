using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations.Comparators;

/// <summary>
/// Compares two string values using either ordinal char-by-char analysis (returning a rich
/// <c>StringDifference</c> that pinpoints the first differing row and column) or any
/// <see cref="StringComparison"/> mode via <see cref="CompareWith"/>. Also implements
/// <see cref="IComparator{T}"/> for integration with the generic comparison pipeline.
/// </summary>
internal class StringComparator : IComparator<string?>
{
    /// <summary>
    /// Compares two strings using ordinal (case-sensitive) comparison.
    /// Returns a detailed StringDifference if they differ.
    /// </summary>
    public static (bool, StringDifference?) Compare(string? actualValue, string? expectedValue)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (actualValue is null && expectedValue is null)
        {
            return (true, null);
        }

        if (actualValue is null && expectedValue is not null)
        {
            return (
                false,
                StringDifference.New(actualValue, expectedValue, DifferenceType.FoundNull)
            );
        }

        if (actualValue is not null && expectedValue is null)
        {
            return (
                false,
                StringDifference.New(actualValue, expectedValue, DifferenceType.NotFoundNull)
            );
        }

        var d = FindDifference(actualValue!, expectedValue!);

        return d is null ? (true, null) : (false, d);
    }

    /// <summary>
    /// Compares two strings using the specified StringComparison.
    /// For Ordinal comparison, delegates to the detailed char-by-char comparison.
    /// For other comparisons, uses string.Equals() directly.
    /// </summary>
    public static (bool, string?) CompareWith(
        string? actualValue,
        string? expectedValue,
        StringComparison comparison)
    {
        if (comparison == StringComparison.Ordinal)
        {
            var result = Compare(actualValue, expectedValue);
            return (result.Item1, result.Item2?.ToString());
        }

        if (string.Equals(actualValue, expectedValue, comparison))
        {
            return (true, null);
        }

        return (false, $"Expected {Format(expectedValue)} but found {Format(actualValue)} (using {comparison}).");
    }

    // IComparator<string?> implementation
    ComparisonResult IComparator<string?>.Compare(string? actual, string? expected)
    {
        var (areEqual, diff) = Compare(actual, expected);
        return areEqual
            ? ComparisonResult.Equal
            : ComparisonResult.NotEqual(diff?.ToString() ?? string.Empty);
    }

    ComparisonResult IComparator<string?>.Compare(
        string? actual,
        string? expected,
        ComparisonOptions options)
    {
        var (areEqual, diffDesc) = CompareWith(actual, expected, options.StringComparison);
        return areEqual
            ? ComparisonResult.Equal
            : ComparisonResult.NotEqual(diffDesc ?? string.Empty);
    }

    private static string Format(string? value)
        => value is null ? "<null>" : $"\"{value}\"";

    #region PRIVATE METHODS

    private static StringDifference? FindDifference(string actualValue, string expectedValue)
    {
        if (
            !actualValue.Contains(Environment.NewLine)
            && !expectedValue.Contains(Environment.NewLine)
        )
        {
            return FindDifferenceByLine(actualValue, expectedValue, 1);
        }

        using var reader1 = new StringReader(actualValue);
        using var reader2 = new StringReader(expectedValue);

        string? line1;
        string? line2;
        var row = 0;

        while ((line1 = reader1.ReadLine()) != null | (line2 = reader2.ReadLine()) != null)
        {
            row++;

            if (line1 is null)
            {
                return StringDifference.New(
                    line1,
                    line2,
                    DifferenceType.MissingLine,
                    Difference.New(row, 0)
                );
            }

            if (line2 is null)
            {
                return StringDifference.New(
                    line1,
                    line2,
                    DifferenceType.AdditionalLine,
                    Difference.New(row, 0)
                );
            }

            var d = FindDifferenceByLine(line1, line2, row);

            if (d is not null)
            {
                return d;
            }
        }

        return null;
    }

    private static StringDifference? FindDifferenceByLine(
        string actualValue,
        string expectedValue,
        int row
    )
    {
        var minLength = Math.Min(actualValue.Length, expectedValue.Length);

        for (var i = 0; i < minLength; i++)
        {
            if (actualValue[i] != expectedValue[i])
            {
                return StringDifference.New(
                    expectedValue,
                    actualValue,
                    DifferenceType.Character,
                    Difference.New(row, i + 1)
                );
            }
        }

        if (actualValue.Length > minLength || expectedValue.Length > minLength)
        {
            return StringDifference.New(
                expectedValue,
                actualValue,
                DifferenceType.Character,
                Difference.New(row, minLength + 1)
            );
        }

        return null;
    }

    #endregion
}
