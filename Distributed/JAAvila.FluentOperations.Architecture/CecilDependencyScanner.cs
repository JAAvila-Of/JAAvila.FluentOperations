using System.Collections.Concurrent;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace JAAvila.FluentOperations.Architecture;

/// <summary>
/// IL-level dependency scanner powered by Mono.Cecil. Detects dependencies
/// that reflection cannot see: local variables, inline <c>new</c> calls,
/// <c>typeof()</c> references, cast expressions, method calls, field access,
/// exception handler catch types, and delegate target types.
/// </summary>
/// <remarks>
/// This scanner performs two passes:
/// <list type="number">
///   <item>Reflection pass: delegates to <see cref="ReflectionDependencyScanner"/>
///         for type signatures (fields, properties, constructors, methods, base types,
///         interfaces, attributes, events, generic arguments).</item>
///   <item>IL pass: scans method body instructions via Mono.Cecil for additional
///         type references not visible through reflection.</item>
/// </list>
/// Install this package and call <see cref="ArchitectureScannerConfig.UseCecilDependencyScanning()"/>
/// to activate IL-level scanning for <c>HaveDependencyOn</c>, <c>NotHaveDependencyOn</c>,
/// and <c>OnlyHaveDependenciesOn</c> operations.
/// </remarks>
public sealed class CecilDependencyScanner : IDependencyScanner, IDisposable
{
    // AssemblyDefinition? allows null sentinel for failed loads
    private readonly ConcurrentDictionary<string, AssemblyDefinition?> _assemblyCache = new();
    private readonly ConcurrentDictionary<string, HashSet<string>> _typeCache = new();
    private bool _disposed;

    /// <summary>
    /// Returns the set of all namespaces that the given type depends on,
    /// combining reflection-level and IL-level analysis.
    /// </summary>
    public HashSet<string> GetReferencedNamespaces(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        ObjectDisposedException.ThrowIf(_disposed, this);

        var cacheKey = type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
        return _typeCache.GetOrAdd(cacheKey, _ => ScanType(type));
    }

    /// <summary>
    /// Checks whether the type has a dependency on any namespace matching the prefix,
    /// using combined reflection + IL analysis.
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
    /// Checks whether all non-BCL dependencies fall within the allowed namespaces,
    /// using combined reflection + IL analysis.
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
    /// Releases all cached <see cref="AssemblyDefinition"/> instances.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        foreach (var assembly in _assemblyCache.Values)
        {
            try
            {
                assembly?.Dispose();
            }
            catch
            {
                /* best effort */
            }
        }

        _assemblyCache.Clear();
        _typeCache.Clear();
    }

    // --- Private methods ---

    private HashSet<string> ScanType(Type type)
    {
        // Pass 1: Reflection (reuse existing implementation)
        var namespaces = ReflectionDependencyScanner.Instance.GetReferencedNamespaces(type);

        // Pass 2: IL inspection via Cecil
        ScanMethodBodiesFromCecil(type, namespaces);

        return namespaces;
    }

    private void ScanMethodBodiesFromCecil(Type type, HashSet<string> namespaces)
    {
        var assemblyLocation = type.Assembly.Location;

        if (string.IsNullOrEmpty(assemblyLocation))
        {
            // Dynamic assembly, no file on disk — fall back to reflection-only
            return;
        }

        var assemblyDef = LoadAssembly(assemblyLocation);

        if (assemblyDef == null)
        {
            // Could not load — fall back to reflection-only
            return;
        }

        var typeDef = FindTypeDefinition(assemblyDef, type);

        if (typeDef == null)
        {
            // Type isn't found in Cecil's view — fall back to reflection-only
            return;
        }

        ScanMethodBodies(typeDef, namespaces);
        ScanCompilerGeneratedNestedTypes(typeDef, namespaces);

        // Remove the type's own namespace — IL pass may re-add it
        namespaces.Remove(type.Namespace ?? string.Empty);
    }

    private AssemblyDefinition? LoadAssembly(string location)
    {
        if (_assemblyCache.TryGetValue(location, out var cached))
        {
            // Maybe null (failed load sentinel)
            return cached;
        }

        AssemblyDefinition? loaded = null;

        try
        {
            var assemblyDir = Path.GetDirectoryName(location) ?? string.Empty;
            var resolver = new CecilAssemblyResolver(assemblyDir);
            var readerParameters = new ReaderParameters
            {
                AssemblyResolver = resolver,
                ReadingMode = ReadingMode.Deferred,
                ReadSymbols = false,
            };
            loaded = AssemblyDefinition.ReadAssembly(location, readerParameters);
        }
        catch
        {
            // Graceful degradation: store null sentinel so we don't retry on every call
        }

        // Store result (null or loaded) as sentinel
        _assemblyCache.TryAdd(location, loaded);
        return loaded;
    }

    private static TypeDefinition? FindTypeDefinition(AssemblyDefinition assembly, Type type)
    {
        // Try by full name (handles nested types via '/' separator in Cecil)
        var fullName = type.FullName;

        if (fullName == null)
        {
            return null;
        }

        // Convert CLR nested type separator '+' to Cecil's '/'
        var cecilName = fullName.Replace('+', '/');

        foreach (var module in assembly.Modules)
        {
            var typeDef = module.GetType(cecilName);

            if (typeDef != null)
            {
                return typeDef;
            }

            // Fallback: search by namespace + name
            typeDef = module.GetType(type.Namespace ?? string.Empty, type.Name);

            if (typeDef != null)
            {
                return typeDef;
            }
        }

        return null;
    }

    private static void ScanMethodBodies(TypeDefinition typeDef, HashSet<string> namespaces)
    {
        foreach (var method in typeDef.Methods.Where(method => method.HasBody))
        {
            foreach (var instruction in method.Body.Instructions)
            {
                switch (instruction.Operand)
                {
                    case TypeReference typeRef:
                        AddCecilTypeNamespace(typeRef, namespaces);
                        break;

                    case MethodReference methodRef:
                        // The declaring type of the method being called
                        AddCecilTypeNamespace(methodRef.DeclaringType, namespaces);
                        // Return type
                        AddCecilTypeNamespace(methodRef.ReturnType, namespaces);
                        // Parameter types
                        if (methodRef.HasParameters)
                        {
                            foreach (var param in methodRef.Parameters)
                            {
                                AddCecilTypeNamespace(param.ParameterType, namespaces);
                            }
                        }
                        // Generic arguments (for GenericInstanceMethod)
                        if (methodRef is GenericInstanceMethod gim)
                        {
                            foreach (var arg in gim.GenericArguments)
                            {
                                AddCecilTypeNamespace(arg, namespaces);
                            }
                        }
                        break;

                    case FieldReference fieldRef:
                        AddCecilTypeNamespace(fieldRef.DeclaringType, namespaces);
                        AddCecilTypeNamespace(fieldRef.FieldType, namespaces);
                        break;
                }
            }

            // Also scan local variables
            if (method.Body.HasVariables)
            {
                foreach (var local in method.Body.Variables)
                    AddCecilTypeNamespace(local.VariableType, namespaces);
            }

            // Scan exception handler catch types
            if (!method.Body.HasExceptionHandlers)
            {
                continue;
            }

            foreach (var handler in method.Body.ExceptionHandlers)
            {
                if (handler.CatchType != null)
                {
                    AddCecilTypeNamespace(handler.CatchType, namespaces);
                }
            }
        }
    }

    private static void ScanCompilerGeneratedNestedTypes(
        TypeDefinition typeDef,
        HashSet<string> namespaces
    )
    {
        if (!typeDef.HasNestedTypes)
        {
            return;
        }

        foreach (var nested in typeDef.NestedTypes.Where(IsCompilerGenerated))
        {
            ScanMethodBodies(nested, namespaces);
        }
    }

    private static void AddCecilTypeNamespace(TypeReference? typeRef, HashSet<string> namespaces)
    {
        if (typeRef == null)
        {
            return;
        }

        // Unwrap modifiers (required/optional)
        if (typeRef is RequiredModifierType reqMod)
        {
            AddCecilTypeNamespace(reqMod.ElementType, namespaces);
            AddCecilTypeNamespace(reqMod.ModifierType, namespaces);
            return;
        }

        if (typeRef is OptionalModifierType optMod)
        {
            AddCecilTypeNamespace(optMod.ElementType, namespaces);
            AddCecilTypeNamespace(optMod.ModifierType, namespaces);
            return;
        }

        // Unwrap an array, pointer, by-ref, pinned
        if (typeRef is ArrayType arrayType)
        {
            AddCecilTypeNamespace(arrayType.ElementType, namespaces);
            return;
        }

        if (typeRef is PointerType pointerType)
        {
            AddCecilTypeNamespace(pointerType.ElementType, namespaces);
            return;
        }

        if (typeRef is ByReferenceType byRefType)
        {
            AddCecilTypeNamespace(byRefType.ElementType, namespaces);
            return;
        }

        if (typeRef is PinnedType pinnedType)
        {
            AddCecilTypeNamespace(pinnedType.ElementType, namespaces);
            return;
        }

        // Handle generic instances (e.g., List<MyEntity>)
        if (typeRef is GenericInstanceType genInst)
        {
            // Add the generic type definition's namespace
            AddCecilTypeNamespace(genInst.ElementType, namespaces);

            // Add each generic argument's namespace
            foreach (var arg in genInst.GenericArguments)
            {
                AddCecilTypeNamespace(arg, namespaces);
            }

            return;
        }

        // Skip generic parameters (T, TKey, etc.) — they are placeholders
        if (typeRef is GenericParameter)
        {
            return;
        }

        // Add the namespace
        var ns = typeRef.Namespace;

        if (!string.IsNullOrEmpty(ns))
        {
            namespaces.Add(ns);
        }

        // For nested types, also add the declaring type's namespace
        if (typeRef.DeclaringType != null)
        {
            AddCecilTypeNamespace(typeRef.DeclaringType, namespaces);
        }
    }

    private static bool IsCompilerGenerated(TypeDefinition typeDef)
    {
        return typeDef.HasCustomAttributes
            && typeDef.CustomAttributes.Any(
                a =>
                    a.AttributeType.FullName
                    == "System.Runtime.CompilerServices.CompilerGeneratedAttribute"
            );
    }
}
