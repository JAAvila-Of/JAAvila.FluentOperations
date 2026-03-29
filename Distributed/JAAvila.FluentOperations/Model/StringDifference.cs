using System.Text;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Formatters;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Model;

internal record StringDifference
{
    private readonly int _maxDisplayLength;
    private const int MinDisplayLength = 10;

    private readonly string? _expectedValue;
    private readonly string? _foundValue;
    private readonly DifferenceType _type;
    private readonly Difference? _difference;

    private StringDifference(
        string? expectedValue,
        string? foundValue,
        DifferenceType type = DifferenceType.None,
        Difference? difference = null
    )
    {
        _maxDisplayLength = GlobalConfig.GetStringConfig().MaxDisplayLength;
        _expectedValue = expectedValue;
        _foundValue = foundValue;
        _type = type;
        _difference = difference;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="StringDifference"/> record with the specified values.
    /// </summary>
    /// <param name="expectedValue">The expected value in the comparison.</param>
    /// <param name="foundValue">The actual value found during the comparison.</param>
    /// <param name="type">The type of the difference encountered.</param>
    /// <param name="difference">The specific details of the difference, if any.</param>
    /// <returns>A new instance of the <see cref="StringDifference"/> record populated with the specified values.</returns>
    public static StringDifference New(
        string? expectedValue,
        string? foundValue,
        DifferenceType type = DifferenceType.None,
        Difference? difference = null
    ) => new(expectedValue, foundValue, type, difference);

    /// <summary>
    /// Returns a string representation of the <see cref="StringDifference"/> instance based on the difference type and its associated details.
    /// </summary>
    /// <returns>A string describing the difference represented by the <see cref="StringDifference"/> instance.</returns>
    public override string ToString()
    {
        return _type switch
        {
            DifferenceType.None => string.Empty,
            DifferenceType.FoundNull
                => $"Expected to find {StringFormatter.Format(_foundValue)} but <null> was found.",
            DifferenceType.NotFoundNull
                => $"Expected to find <null> but found {StringFormatter.Format(_expectedValue)}",
            DifferenceType.AdditionalLine
                => $"The current value has additional lines. Line {_difference?.Row ?? 1} has been found which was not expected",
            DifferenceType.MissingLine
                => $"The current value has missing lines. Line {_difference?.Row ?? 1} was expected but has not been found.",
            DifferenceType.Character => BuildMessageByCharacterDifference(),
            _ => string.Empty
        };
    }

    #region PRIVATE METHODS

    private string BuildMessageByCharacterDifference()
    {
        if (_expectedValue is null || _foundValue is null || _difference is null)
        {
            return string.Empty;
        }

        var expectedValueLength = _expectedValue.Length;
        var foundValueLength = _foundValue.Length;
        var minLength = Math.Min(expectedValueLength, foundValueLength);
        var maxLength = Math.Max(expectedValueLength, foundValueLength);
        var difLength = Math.Abs(expectedValueLength - foundValueLength);

        if (maxLength < MinDisplayLength)
        {
            return $"Expected to find {StringFormatter.Format(_expectedValue)} but found {StringFormatter.Format(_foundValue)}.";
        }

        var row = _difference.Row;
        var index = _difference.Index;
        var middle = Convert.ToInt32(Math.Floor(_maxDisplayLength / 2D));
        var hasEllipsisInPrefix = index > middle && minLength > _maxDisplayLength;
        var startIndex =
            index <= middle || minLength <= _maxDisplayLength
                ? 0
                : minLength - _maxDisplayLength < index
                    ? minLength - _maxDisplayLength + (difLength >= 5 ? 5 : difLength)
                    : index - middle;

        var sb = new StringBuilder();
        var foundDisplay = _foundValue.SafeSubstring(startIndex, _maxDisplayLength, false);
        var expectedDisplay = _expectedValue.SafeSubstring(startIndex, _maxDisplayLength, false);

        sb.Append(
                $"A difference has been found {(row > 1 ? $"in line {row} and" : "at")} index {index - 1}:"
            )
            .AppendLine();

        AppendDifferenceIndicators(
            sb,
            hasEllipsisInPrefix,
            index,
            startIndex,
            "Found",
            Unicodes.ArrowDown
        );

        FormatFoundValueDisplay(
            sb,
            hasEllipsisInPrefix,
            foundDisplay,
            startIndex,
            foundValueLength
        );

        FormatExpectedValueDisplay(
            sb,
            hasEllipsisInPrefix,
            expectedDisplay,
            startIndex,
            expectedValueLength
        );

        AppendDifferenceIndicators(
            sb,
            hasEllipsisInPrefix,
            index,
            startIndex,
            "Expected",
            Unicodes.ArrowUp
        );

        return sb.ToString();
    }

    private static void FormatExpectedValueDisplay(
        StringBuilder sb,
        bool hasEllipsisInPrefix,
        string expectedDisplay,
        int startIndex,
        int expectedValueLength
    )
    {
        sb.Append(Unicodes.Tab)
            .Append(Unicodes.Tab)
            .Append($"\"{(hasEllipsisInPrefix ? Unicodes.Ellipsis : string.Empty)}")
            .Append($"{expectedDisplay}")
            .Append(
                $"{(startIndex + expectedDisplay.Length < expectedValueLength ? Unicodes.Ellipsis : string.Empty)}\""
            )
            .Append($" (Length: {expectedValueLength})")
            .AppendLine();
    }

    private static void FormatFoundValueDisplay(
        StringBuilder sb,
        bool hasEllipsisInPrefix,
        string foundDisplay,
        int startIndex,
        int foundValueLength
    )
    {
        sb.Append(Unicodes.Tab)
            .Append(Unicodes.Tab)
            .Append($"\"{(hasEllipsisInPrefix ? Unicodes.Ellipsis : string.Empty)}")
            .Append($"{foundDisplay}")
            .Append(
                $"{(startIndex + foundDisplay.Length < foundValueLength ? Unicodes.Ellipsis : string.Empty)}\""
            )
            .Append($" (Length: {foundValueLength})")
            .AppendLine();
    }

    private static void AppendDifferenceIndicators(
        StringBuilder sb,
        bool hasEllipsisInPrefix,
        int index,
        int startIndex,
        string section,
        char indicator
    )
    {
        sb.Append(Unicodes.Tab)
            .Append(Unicodes.Tab)
            .Append($" {(hasEllipsisInPrefix ? " " : string.Empty)}")
            .AppendJoin(' ', new string[index - startIndex])
            .Append($"{indicator}")
            .Append($" ({section})")
            .AppendLine();
    }

    #endregion
}
