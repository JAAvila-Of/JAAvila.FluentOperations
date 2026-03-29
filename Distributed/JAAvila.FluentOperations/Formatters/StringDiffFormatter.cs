using JAAvila.FluentOperations.Config;

namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Formats a human-readable diff between two strings, highlighting the first point of divergence
/// with context characters on each side and a caret pointer at the difference index.
/// </summary>
internal static class StringDiffFormatter
{
    /// <summary>
    /// Formats a diff between <paramref name="expected"/> and <paramref name="actual"/>,
    /// reading context and length limits from <see cref="GlobalConfig"/>.
    /// Returns <c>null</c> when no diff should be shown.
    /// </summary>
    public static string? FormatDiff(string? expected, string? actual)
    {
        var config = GlobalConfig.GetStringConfig();
        return FormatDiff(expected, actual, config.StringDiffContextChars, config.StringDiffMaxLength);
    }

    /// <summary>
    /// Formats a diff between <paramref name="expected"/> and <paramref name="actual"/> using the
    /// supplied <paramref name="contextChars"/> window and <paramref name="maxLength"/> cap.
    /// Returns <c>null</c> when no diff should be shown (null inputs, equal strings, or truncation).
    /// </summary>
    public static string? FormatDiff(string? expected, string? actual, int contextChars = 20, int maxLength = 1000)
    {
        if (expected is null || actual is null)
        {
            return null;
        }

        if (string.Equals(expected, actual, StringComparison.Ordinal))
        {
            return null;
        }

        if (expected.Length > maxLength || actual.Length > maxLength)
        {
            return $"(strings too long to diff: expected length {expected.Length}, actual length {actual.Length}, max {maxLength})";
        }

        var diffIndex = FindFirstDifferenceIndex(expected, actual);

        // Build context window
        var start = Math.Max(0, diffIndex - contextChars);
        var expectedEnd = Math.Min(expected.Length, diffIndex + contextChars);
        var actualEnd = Math.Min(actual.Length, diffIndex + contextChars);

        var expectedSnippet = SafeSubstring(expected, start, expectedEnd - start);
        var actualSnippet = SafeSubstring(actual, start, actualEnd - start);

        var prefixEllipsis = start > 0 ? "..." : string.Empty;
        var expectedSuffixEllipsis = expectedEnd < expected.Length ? "..." : string.Empty;
        var actualSuffixEllipsis = actualEnd < actual.Length ? "..." : string.Empty;

        // Caret position relative to the snippet (offset by ellipsis width)
        var caretOffset = diffIndex - start + prefixEllipsis.Length;
        var caret = new string(' ', caretOffset) + "^";

        var expectedWord = ExtractWordAround(expected, diffIndex);
        var actualWord = ExtractWordAround(actual, diffIndex);

        return
            $"Difference at index {diffIndex}:" + Environment.NewLine +
            $"  Expected : \"{prefixEllipsis}{expectedSnippet}{expectedSuffixEllipsis}\"" + Environment.NewLine +
            $"  But found: \"{prefixEllipsis}{actualSnippet}{actualSuffixEllipsis}\"" + Environment.NewLine +
            $"             {caret}" + Environment.NewLine +
            $"  (expected \"{expectedWord}\" but found \"{actualWord}\" near index {diffIndex})";
    }

    #region PRIVATE HELPERS

    private static int FindFirstDifferenceIndex(string a, string b)
    {
        var minLen = Math.Min(a.Length, b.Length);
        
        for (var i = 0; i < minLen; i++)
        {
            if (a[i] != b[i])
            {
                return i;
            }
        }

        return minLen;
    }

    private static string SafeSubstring(string value, int start, int length)
    {
        if (start >= value.Length)
        {
            return string.Empty;
        }

        var safeLength = Math.Min(length, value.Length - start);
        return safeLength <= 0 ? string.Empty : value.Substring(start, safeLength);
    }

    private static string ExtractWordAround(string value, int index)
    {
        if (index >= value.Length)
        {
            return string.Empty;
        }

        // Walk back to the start of the word
        var start = index;
        
        while (start > 0 && !char.IsWhiteSpace(value[start - 1]))
        {
            start--;
        }

        // Walk forward to the end of the word
        var end = index;
        
        while (end < value.Length && !char.IsWhiteSpace(value[end]))
        {
            end++;
        }

        var word = value[start..end];
        return word.Length > 20 ? word[..20] + "..." : word;
    }

    #endregion
}
