namespace JAAvila.FluentOperations.Utils;

/// <summary>
/// Provides wildcard pattern matching using '*' as a multi-character wildcard.
/// </summary>
internal static class WildcardMatcher
{
    /// <summary>
    /// Determines whether the specified value matches the wildcard pattern.
    /// Use '*' as a wildcard that matches any sequence of characters.
    /// Without any '*', performs an exact match.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <param name="pattern">The wildcard pattern, where '*' matches any sequence of characters.</param>
    /// <param name="comparison">The string comparison type. Defaults to Ordinal.</param>
    /// <returns>True if the value matches the pattern; otherwise false.</returns>
    public static bool IsMatch(
        string value,
        string pattern,
        StringComparison comparison = StringComparison.Ordinal
    )
    {
        if (string.IsNullOrEmpty(pattern))
        {
            return string.IsNullOrEmpty(value);
        }

        // Split by '*' and verify each segment appears in order
        var segments = pattern.Split('*', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length == 0)
        {
            return true; // Pattern is all wildcards
        }

        var currentIndex = 0;

        // If a pattern doesn't start with '*', the first segment must be at the start
        if (!pattern.StartsWith('*'))
        {
            if (!value.StartsWith(segments[0], comparison))
            {
                return false;
            }

            currentIndex = segments[0].Length;
            segments = segments.Skip(1).ToArray();
        }

        foreach (var segment in segments)
        {
            var foundIndex = value.IndexOf(segment, currentIndex, comparison);

            if (foundIndex < 0)
            {
                return false;
            }

            currentIndex = foundIndex + segment.Length;
        }

        // If a pattern doesn't end with '*', the last segment must be at the end
        if (!pattern.EndsWith('*') && segments.Length > 0)
        {
            var lastSegment = segments[^1];
            return value.EndsWith(lastSegment, comparison);
        }

        return true;
    }
}
