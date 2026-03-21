using System.Text.Json;

namespace JAAvila.FluentOperations.Schema;

/// <summary>
/// Maps .NET CLR types to their JSON Schema <c>type</c> and <c>format</c> equivalents.
/// Writes directly to a <see cref="Utf8JsonWriter"/> to avoid intermediate allocations.
/// </summary>
internal static class JsonSchemaTypeMapper
{
    /// <summary>
    /// Writes the <c>type</c> (and optionally <c>format</c>) properties for the given CLR type.
    /// For nullable value types, writes a two-element type array: <c>["type","null"]</c>.
    /// For collection types, writes <c>type: "array"</c> and an <c>items</c> object with the
    /// element type. For unknown types, writes <c>type: "object"</c>.
    /// </summary>
    /// <param name="writer">The JSON writer to write into. Must be positioned inside an object.</param>
    /// <param name="type">The CLR type to map.</param>
    internal static void WriteTypeProperties(Utf8JsonWriter writer, Type type)
    {
        // Unwrap Nullable<T>
        var underlying = Nullable.GetUnderlyingType(type);
        if (underlying is not null)
        {
            WriteNullableTypeProperties(writer, underlying);
            return;
        }

        // Collection types (IEnumerable<T> but not string)
        if (type != typeof(string) && TryGetCollectionElementType(type, out var elementType))
        {
            writer.WriteString("type", "array");
            writer.WritePropertyName("items");
            writer.WriteStartObject();
            WriteTypeProperties(writer, elementType!);
            writer.WriteEndObject();
            return;
        }

        WriteSimpleTypeProperties(writer, type);
    }

    private static void WriteNullableTypeProperties(Utf8JsonWriter writer, Type underlyingType)
    {
        // Collect what the type would be without null
        var (jsonType, format) = GetTypeAndFormat(underlyingType);

        writer.WritePropertyName("type");
        writer.WriteStartArray();
        writer.WriteStringValue(jsonType);
        writer.WriteStringValue("null");
        writer.WriteEndArray();

        if (format is not null)
        {
            writer.WriteString("format", format);
        }

        // Enum values
        if (underlyingType.IsEnum)
        {
            WriteEnumValues(writer, underlyingType);
        }
    }

    private static void WriteSimpleTypeProperties(Utf8JsonWriter writer, Type type)
    {
        var (jsonType, format) = GetTypeAndFormat(type);

        writer.WriteString("type", jsonType);

        if (format is not null)
        {
            writer.WriteString("format", format);
        }

        if (type.IsEnum)
        {
            WriteEnumValues(writer, type);
        }
    }

    /// <summary>
    /// Returns the JSON Schema type string and optional format string for a CLR type.
    /// Does not handle Nullable&lt;T&gt; or collections — those are handled by the callers.
    /// </summary>
    private static (string JsonType, string? Format) GetTypeAndFormat(Type type)
    {
        if (type == typeof(string)) return ("string", null);
        if (type == typeof(bool)) return ("boolean", null);
        if (type == typeof(int)) return ("integer", "int32");
        if (type == typeof(long)) return ("integer", "int64");
        if (type == typeof(float)) return ("number", "float");
        if (type == typeof(double)) return ("number", "double");
        if (type == typeof(decimal)) return ("number", "decimal");
        if (type == typeof(DateTime)) return ("string", "date-time");
        if (type == typeof(DateTimeOffset)) return ("string", "date-time");
        if (type == typeof(DateOnly)) return ("string", "date");
        if (type == typeof(TimeOnly)) return ("string", "time");
        if (type == typeof(Guid)) return ("string", "uuid");
        if (type == typeof(Uri)) return ("string", "uri");
        if (type.IsEnum) return ("string", null);

        return ("object", null);
    }

    private static void WriteEnumValues(Utf8JsonWriter writer, Type enumType)
    {
        writer.WritePropertyName("enum");
        writer.WriteStartArray();
        foreach (var name in Enum.GetNames(enumType))
        {
            writer.WriteStringValue(name);
        }
        writer.WriteEndArray();
    }

    private static bool TryGetCollectionElementType(Type type, out Type? elementType)
    {
        // Array
        if (type.IsArray)
        {
            elementType = type.GetElementType()!;
            return true;
        }

        // IEnumerable<T> or any interface/class implementing it
        foreach (var iface in type.GetInterfaces())
        {
            if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                elementType = iface.GetGenericArguments()[0];
                return true;
            }
        }

        elementType = null;
        return false;
    }
}
