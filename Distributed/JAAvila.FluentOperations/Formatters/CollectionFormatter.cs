using System.Collections;
using System.Text;

namespace JAAvila.FluentOperations.Formatters;

/// <summary>
/// Formatter for collection types: arrays, lists, and any IEnumerable.
/// Truncates display after MaxCollectionItems with a count indicator.
/// </summary>
internal class CollectionFormatter : IValueFormatter
{
    public bool CanHandle(Type type)
    {
        return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
    }

    public string Format(object? value, FormattingContext context)
    {
        if (value is null)
        {
            return context.NullDisplay;
        }

        if (context.CurrentDepth >= context.MaxDepth)
        {
            return "[...]";
        }

        if (value is not IEnumerable enumerable)
        {
            return value.ToString() ?? context.NullDisplay;
        }

        var sb = new StringBuilder("[");
        var childContext = context.Descend();
        var count = 0;
        var totalCount = 0;
        var hasMore = false;

        foreach (var item in enumerable)
        {
            totalCount++;

            if (count >= context.MaxCollectionItems)
            {
                hasMore = true;
                continue;
            }

            if (count > 0)
            {
                sb.Append(", ");
            }

            var itemType = item?.GetType() ?? typeof(object);
            sb.Append(FormatterPipeline.FormatWithContext(item, itemType, childContext));
            count++;
        }

        if (hasMore)
        {
            sb.Append($", ...({totalCount} items)");
        }

        sb.Append(']');
        return sb.ToString();
    }

    public int Priority => 50;
}
