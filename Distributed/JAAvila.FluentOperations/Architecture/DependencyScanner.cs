using System.Reflection;

namespace JAAvila.FluentOperations.Architecture;

/// <summary>
/// Reflection-based dependency scanner. Scans a <see cref="Type"/> via reflection
/// to discover all namespaces it depends on.
/// Inspects: fields, properties, method parameters, method return types,
/// constructor parameters, base types, interfaces, generic type arguments,
/// event handler types, and custom attribute types.
/// </summary>
/// <remarks>
/// <para>LIMITATIONS (documented by design):</para>
/// <list type="bullet">
///   <item>Does NOT inspect method body IL — local variables, inline <c>new</c> calls,
///         casts, and typeof() inside method bodies are NOT detected.</item>
///   <item>Does NOT detect dependencies accessed only through dynamic dispatch or reflection.</item>
///   <item>Does NOT follow transitive dependencies (only direct type surface).</item>
/// </list>
/// For IL-level analysis, install the <c>JAAvila.FluentOperations.Architecture</c> package
/// and call <c>ArchitectureScannerConfig.UseCecilDependencyScanning()</c>.
/// </remarks>
internal sealed class ReflectionDependencyScanner : IDependencyScanner
{
    /// <summary>
    /// The singleton instance of <see cref="ReflectionDependencyScanner"/>.
    /// </summary>
    public static readonly ReflectionDependencyScanner Instance = new();

    private ReflectionDependencyScanner() { }

    private const BindingFlags AllMembers =
        BindingFlags.Public
        | BindingFlags.NonPublic
        | BindingFlags.Instance
        | BindingFlags.Static
        | BindingFlags.DeclaredOnly;

    /// <summary>
    /// Returns the set of all namespaces that the given type depends on,
    /// based on its public API surface and declarations.
    /// </summary>
    public HashSet<string> GetReferencedNamespaces(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        var namespaces = new HashSet<string>(StringComparer.Ordinal);

        CollectFromBaseType(type, namespaces);
        CollectFromInterfaces(type, namespaces);
        CollectFromFields(type, namespaces);
        CollectFromProperties(type, namespaces);
        CollectFromConstructors(type, namespaces);
        CollectFromMethods(type, namespaces);
        CollectFromEvents(type, namespaces);
        CollectFromAttributes(type, namespaces);

        // Remove the type's own namespace — a type does not "depend on" itself
        namespaces.Remove(type.Namespace ?? string.Empty);
        // NOTE: We do NOT strip System.* by default — the user decides what matters

        return namespaces;
    }

    /// <summary>
    /// Checks whether the type has a dependency on ANY namespace that starts
    /// with the given prefix. Supports both exact match ("MyApp.Domain")
    /// and prefix match ("MyApp.Domain" matches "MyApp.Domain.Entities").
    /// </summary>
    public bool HasDependencyOnNamespace(Type type, string namespacePrefix)
    {
        var namespaces = GetReferencedNamespaces(type);
        return namespaces.Any(
            ns =>
                string.Equals(ns, namespacePrefix, StringComparison.Ordinal)
                || ns.StartsWith(namespacePrefix + ".", StringComparison.Ordinal)
        );
    }

    /// <summary>
    /// Checks whether ALL the type's dependencies fall within the allowed
    /// namespace list. Each allowed namespace acts as a prefix.
    /// The type's own namespace is always implicitly allowed.
    /// System.* and Microsoft.* namespaces are always implicitly allowed (BCL is not a "dependency").
    /// </summary>
    public (bool isCompliant, IReadOnlyList<string> violatingNamespaces) CheckOnlyDependenciesOn(
        Type type,
        IReadOnlyList<string> allowedNamespaces
    )
    {
        var namespaces = GetReferencedNamespaces(type);
        var violating = (
            from ns in namespaces
            where
                !ns.StartsWith("System", StringComparison.Ordinal)
                && !ns.StartsWith("Microsoft", StringComparison.Ordinal)
            let isAllowed = allowedNamespaces.Any(
                allowed =>
                    string.Equals(ns, allowed, StringComparison.Ordinal)
                    || ns.StartsWith(allowed + ".", StringComparison.Ordinal)
            )
            where !isAllowed
            select ns
        ).ToList();

        return (violating.Count == 0, violating);
    }

    /// <summary>
    /// Returns the set of all fully qualified type names that the given type depends on,
    /// based on its public API surface and declarations.
    /// </summary>
    public HashSet<string> GetReferencedTypes(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        var types = new HashSet<string>(StringComparer.Ordinal);

        CollectTypesFromBaseType(type, types);
        CollectTypesFromInterfaces(type, types);
        CollectTypesFromFields(type, types);
        CollectTypesFromProperties(type, types);
        CollectTypesFromConstructors(type, types);
        CollectTypesFromMethods(type, types);
        CollectTypesFromEvents(type, types);
        CollectTypesFromAttributes(type, types);

        types.Remove(type.FullName ?? string.Empty);
        return types;
    }

    /// <summary>
    /// Checks whether the type has a dependency on the specific type identified by
    /// its fully qualified name.
    /// </summary>
    public bool HasDependencyOnType(Type type, string fullyQualifiedTypeName)
    {
        var referenced = GetReferencedTypes(type);
        return referenced.Contains(fullyQualifiedTypeName);
    }

    // --- Private collection methods ---

    private static void CollectFromBaseType(Type type, HashSet<string> namespaces)
    {
        var baseType = type.BaseType;

        if (
            baseType != null
            && baseType != typeof(object)
            && baseType != typeof(ValueType)
            && baseType != typeof(Enum)
        )
        {
            AddTypeNamespace(baseType, namespaces);
        }
    }

    private static void CollectFromInterfaces(Type type, HashSet<string> namespaces)
    {
        // GetInterfaces() returns ALL interfaces including inherited ones.
        // For dependency analysis, all interfaces are relevant.
        foreach (var iface in type.GetInterfaces())
        {
            AddTypeNamespace(iface, namespaces);
        }
    }

    private static void CollectFromFields(Type type, HashSet<string> namespaces)
    {
        foreach (var field in type.GetFields(AllMembers))
        {
            AddTypeNamespace(field.FieldType, namespaces);
        }
    }

    private static void CollectFromProperties(Type type, HashSet<string> namespaces)
    {
        foreach (var prop in type.GetProperties(AllMembers))
        {
            AddTypeNamespace(prop.PropertyType, namespaces);

            // Also scan indexer parameters
            foreach (var param in prop.GetIndexParameters())
            {
                AddTypeNamespace(param.ParameterType, namespaces);
            }
        }
    }

    private static void CollectFromConstructors(Type type, HashSet<string> namespaces)
    {
        foreach (
            var param in type.GetConstructors(AllMembers).SelectMany(ctor => ctor.GetParameters())
        )
        {
            AddTypeNamespace(param.ParameterType, namespaces);
        }
    }

    private static void CollectFromMethods(Type type, HashSet<string> namespaces)
    {
        foreach (var method in type.GetMethods(AllMembers))
        {
            // Include return types
            AddTypeNamespace(method.ReturnType, namespaces);
            foreach (var param in method.GetParameters())
            {
                AddTypeNamespace(param.ParameterType, namespaces);
            }

            // Generic method type arguments
            if (!method.IsGenericMethod)
            {
                continue;
            }

            foreach (
                var constraint in method
                    .GetGenericArguments()
                    .SelectMany(genArg => genArg.GetGenericParameterConstraints())
            )
            {
                AddTypeNamespace(constraint, namespaces);
            }
        }
    }

    private static void CollectFromEvents(Type type, HashSet<string> namespaces)
    {
        foreach (var evt in type.GetEvents(AllMembers))
        {
            if (evt.EventHandlerType != null)
            {
                AddTypeNamespace(evt.EventHandlerType, namespaces);
            }
        }
    }

    private static void CollectFromAttributes(Type type, HashSet<string> namespaces)
    {
        foreach (var attr in type.GetCustomAttributes(false))
        {
            AddTypeNamespace(attr.GetType(), namespaces);
        }
    }

    // --- Private collection methods for type-level (FullName) dependency scanning ---

    private static void CollectTypesFromBaseType(Type type, HashSet<string> types)
    {
        var baseType = type.BaseType;

        if (
            baseType != null
            && baseType != typeof(object)
            && baseType != typeof(ValueType)
            && baseType != typeof(Enum)
        )
        {
            AddTypeFullName(baseType, types);
        }
    }

    private static void CollectTypesFromInterfaces(Type type, HashSet<string> types)
    {
        foreach (var iface in type.GetInterfaces())
        {
            AddTypeFullName(iface, types);
        }
    }

    private static void CollectTypesFromFields(Type type, HashSet<string> types)
    {
        foreach (var field in type.GetFields(AllMembers))
        {
            AddTypeFullName(field.FieldType, types);
        }
    }

    private static void CollectTypesFromProperties(Type type, HashSet<string> types)
    {
        foreach (var prop in type.GetProperties(AllMembers))
        {
            AddTypeFullName(prop.PropertyType, types);

            foreach (var param in prop.GetIndexParameters())
            {
                AddTypeFullName(param.ParameterType, types);
            }
        }
    }

    private static void CollectTypesFromConstructors(Type type, HashSet<string> types)
    {
        foreach (
            var param in type.GetConstructors(AllMembers).SelectMany(ctor => ctor.GetParameters())
        )
        {
            AddTypeFullName(param.ParameterType, types);
        }
    }

    private static void CollectTypesFromMethods(Type type, HashSet<string> types)
    {
        foreach (var method in type.GetMethods(AllMembers))
        {
            AddTypeFullName(method.ReturnType, types);

            foreach (var param in method.GetParameters())
            {
                AddTypeFullName(param.ParameterType, types);
            }

            if (!method.IsGenericMethod)
            {
                continue;
            }

            foreach (
                var constraint in method
                    .GetGenericArguments()
                    .SelectMany(genArg => genArg.GetGenericParameterConstraints())
            )
            {
                AddTypeFullName(constraint, types);
            }
        }
    }

    private static void CollectTypesFromEvents(Type type, HashSet<string> types)
    {
        foreach (var evt in type.GetEvents(AllMembers))
        {
            if (evt.EventHandlerType != null)
            {
                AddTypeFullName(evt.EventHandlerType, types);
            }
        }
    }

    private static void CollectTypesFromAttributes(Type type, HashSet<string> types)
    {
        foreach (var attr in type.GetCustomAttributes(false))
        {
            AddTypeFullName(attr.GetType(), types);
        }
    }

    /// <summary>
    /// Adds the fully qualified name of a type and recursively handles generic type arguments,
    /// arrays, pointers, and by-ref types.
    /// </summary>
    private static void AddTypeFullName(Type type, HashSet<string> types)
    {
        if (type.IsArray || type.IsByRef || type.IsPointer)
        {
            AddTypeFullName(type.GetElementType()!, types);
            return;
        }

        if (type.FullName != null)
        {
            types.Add(type.FullName);
        }

        if (!type.IsGenericType)
        {
            return;
        }

        if (!type.IsGenericTypeDefinition)
        {
            var genDef = type.GetGenericTypeDefinition();

            if (genDef.FullName != null)
            {
                types.Add(genDef.FullName);
            }
        }

        foreach (var arg in type.GetGenericArguments().Where(arg => !arg.IsGenericParameter))
        {
            AddTypeFullName(arg, types);
        }
    }

    /// <summary>
    /// Adds the namespace of a type and recursively handles generic type arguments,
    /// arrays, pointers, and by-ref types.
    /// </summary>
    private static void AddTypeNamespace(Type type, HashSet<string> namespaces)
    {
        // Unwrap an array, pointer, by-ref
        if (type.IsArray)
        {
            AddTypeNamespace(type.GetElementType()!, namespaces);
            return;
        }

        if (type.IsByRef || type.IsPointer)
        {
            AddTypeNamespace(type.GetElementType()!, namespaces);
            return;
        }

        // Add the type's own namespace
        if (type.Namespace != null)
        {
            namespaces.Add(type.Namespace);
        }

        // Recurse into generic type arguments (e.g., List<MyApp.Domain.Entity>)
        if (!type.IsGenericType)
        {
            return;
        }

        // Add the generic type definition's namespace
        if (!type.IsGenericTypeDefinition)
        {
            var genDef = type.GetGenericTypeDefinition();
            if (genDef.Namespace != null)
                namespaces.Add(genDef.Namespace);
        }

        foreach (var arg in type.GetGenericArguments().Where(arg => !arg.IsGenericParameter))
        {
            AddTypeNamespace(arg, namespaces);
        }
    }
}
