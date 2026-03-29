namespace JAAvila.FluentOperations.Architecture;

/// <summary>
/// Contract for dependency scanners that analyze a <see cref="Type"/> and return
/// the set of namespaces it depends on. Implementations may use reflection,
/// IL inspection, or other analysis techniques.
/// </summary>
/// <remarks>
/// The default implementation (<see cref="ReflectionDependencyScanner"/>) uses .NET reflection,
/// which covers type signatures (fields, properties, constructors, methods, base types,
/// interfaces, attributes, events, generic arguments). For deeper analysis including
/// method body IL (local variables, inline <c>new</c> calls, casts, <c>typeof</c>),
/// install the <c>JAAvila.FluentOperations.Architecture</c> package and call
/// <c>ArchitectureScannerConfig.UseCecilDependencyScanning()</c>.
/// </remarks>
public interface IDependencyScanner
{
    /// <summary>
    /// Returns the set of all namespaces that the given type depends on.
    /// The type's own namespace is excluded from the result.
    /// </summary>
    /// <param name="type">The type to scan for dependencies.</param>
    /// <returns>A set of namespace strings the type depends on.</returns>
    HashSet<string> GetReferencedNamespaces(Type type);

    /// <summary>
    /// Checks whether the type has a dependency on any namespace that starts
    /// with (or exactly matches) the given prefix.
    /// </summary>
    /// <param name="type">The type to scan.</param>
    /// <param name="namespacePrefix">The namespace or namespace prefix to check.</param>
    /// <returns><c>true</c> if a dependency on the namespace is found.</returns>
    bool HasDependencyOnNamespace(Type type, string namespacePrefix);

    /// <summary>
    /// Checks whether all the type's non-BCL dependencies fall within the
    /// allowed namespace list. Each allowed namespace acts as a prefix.
    /// The type's own namespace and BCL namespaces (System.*, Microsoft.*) are
    /// always implicitly allowed.
    /// </summary>
    /// <param name="type">The type to scan.</param>
    /// <param name="allowedNamespaces">The list of allowed namespace prefixes.</param>
    /// <returns>A tuple: (<c>isCompliant</c>, <c>violatingNamespaces</c>).</returns>
    (bool isCompliant, IReadOnlyList<string> violatingNamespaces) CheckOnlyDependenciesOn(
        Type type,
        IReadOnlyList<string> allowedNamespaces
    );

    /// <summary>
    /// Returns the set of all fully qualified type names that the given type depends on.
    /// Default implementation returns an empty set for backwards compatibility;
    /// override for precise type-level dependency scanning.
    /// </summary>
    /// <param name="type">The type to scan for dependencies.</param>
    /// <returns>A set of fully qualified type names.</returns>
    HashSet<string> GetReferencedTypes(Type type) => new HashSet<string>(StringComparer.Ordinal);

    /// <summary>
    /// Checks whether the type has a dependency on the specific type identified by
    /// its fully qualified name (namespace and type name).
    /// </summary>
    /// <param name="type">The type to scan.</param>
    /// <param name="fullyQualifiedTypeName">The fully qualified type name to check for.</param>
    /// <returns><c>true</c> if the type has a dependency on the specified type.</returns>
    bool HasDependencyOnType(Type type, string fullyQualifiedTypeName)
    {
        var referenced = GetReferencedTypes(type);
        return referenced.Contains(fullyQualifiedTypeName);
    }
}
