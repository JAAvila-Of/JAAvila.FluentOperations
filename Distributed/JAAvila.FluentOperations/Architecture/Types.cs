using System.Reflection;

namespace JAAvila.FluentOperations.Architecture;

/// <summary>
/// Provides static entry points for selecting types from assemblies, enabling architecture test assertions.
/// Returns <see cref="IEnumerable{Type}"/> that can be piped into <c>.Test()</c> to get a
/// <c>CollectionOperationsManager&lt;Type&gt;</c> for collection-level assertions, or filtered
/// with <see cref="That"/> before testing.
/// </summary>
/// <example>
/// <code>
/// // layer isolation
/// Types.InAssembly(typeof(Startup).Assembly)
///     .That(t => t.Namespace?.StartsWith("MyApp.Domain") == true)
///     .Test()
///     .AllSatisfy(t => !t.Namespace?.Contains("Infrastructure") == true);
///
/// // Naming convention enforcement
/// Types.InNamespace("MyApp.Controllers")
///     .Test()
///     .AllSatisfy(t => t.Name.EndsWith("Controller"));
/// </code>
/// </example>
public static class Types
{
    /// <summary>
    /// Returns all exported (public) types defined in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan for types.</param>
    /// <returns>An enumerable of all exported types in the assembly.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> is null.</exception>
    /// <remarks>
    /// Uses <see cref="Assembly.GetExportedTypes()"/> which returns only publicly visible types.
    /// For assemblies that contain types with unresolvable dependencies, consider using
    /// <see cref="InAssemblySafe(Assembly)"/> which handles <see cref="ReflectionTypeLoadException"/>.
    /// </remarks>
    public static IEnumerable<Type> InAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        return assembly.GetExportedTypes();
    }

    /// <summary>
    /// Returns all exported (public) types in the assembly that match the predicate.
    /// </summary>
    /// <param name="assembly">The assembly to scan for types.</param>
    /// <param name="predicate">A filter applied to each type.</param>
    /// <returns>An enumerable of matching types.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> or <paramref name="predicate"/> is null.</exception>
    public static IEnumerable<Type> InAssembly(Assembly assembly, Func<Type, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(predicate);
        return assembly.GetExportedTypes().Where(predicate);
    }

    /// <summary>
    /// Returns all types in the assembly, safely handling <see cref="ReflectionTypeLoadException"/>.
    /// Types that fail to load are silently excluded from the result.
    /// </summary>
    /// <param name="assembly">The assembly to scan for types.</param>
    /// <returns>An enumerable of all loadable types in the assembly.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> is null.</exception>
    public static IEnumerable<Type> InAssemblySafe(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t != null)!;
        }
    }

    /// <summary>
    /// Returns all exported types from all assemblies loaded in the current <see cref="AppDomain"/>
    /// that reside in the specified namespace (exact match).
    /// </summary>
    /// <param name="ns">The namespace to filter by (exact match).</param>
    /// <returns>An enumerable of types in the specified namespace.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ns"/> is null.</exception>
    /// <remarks>
    /// Scans <see cref="AppDomain.CurrentDomain"/> loaded assemblies. Types from dynamic assemblies
    /// or assemblies that throw on <c>GetExportedTypes()</c> are silently skipped.
    /// </remarks>
    public static IEnumerable<Type> InNamespace(string ns)
    {
        ArgumentNullException.ThrowIfNull(ns);

        return AppDomain
            .CurrentDomain.GetAssemblies()
            .SelectMany(SafeGetExportedTypes)
            .Where(t => string.Equals(t.Namespace, ns, StringComparison.Ordinal));
    }

    /// <summary>
    /// Returns all exported types that share the same namespace as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">A type whose namespace is used as the filter.</typeparam>
    /// <returns>An enumerable of types in the same namespace as <typeparamref name="T"/>.</returns>
    public static IEnumerable<Type> InNamespaceOf<T>()
    {
        var ns = typeof(T).Namespace;

        return ns is null ? [] : InNamespace(ns);
    }

    /// <summary>
    /// Returns all exported types from all assemblies in the current <see cref="AppDomain"/>.
    /// </summary>
    /// <returns>An enumerable of all exported types in the current domain.</returns>
    /// <remarks>
    /// May return a large number of types. Consider filtering with <see cref="That"/> before calling <c>.Test()</c>.
    /// </remarks>
    public static IEnumerable<Type> InCurrentDomain()
    {
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(SafeGetExportedTypes);
    }

    /// <summary>
    /// Returns all exported (public) types defined in any of the specified assemblies.
    /// Duplicate types across assemblies are included only once (by reference equality).
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for types.</param>
    /// <returns>A distinct enumerable of all exported types across all assemblies.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assemblies"/> is null.</exception>
    public static IEnumerable<Type> InAssemblies(params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);
        return assemblies.SelectMany(a => a.GetExportedTypes()).Distinct();
    }

    /// <summary>
    /// Returns all exported (public) types defined in any of the specified assemblies
    /// that match the predicate.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for types.</param>
    /// <param name="predicate">A filter applied to each type.</param>
    /// <returns>A distinct, filtered enumerable of types.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assemblies"/> or <paramref name="predicate"/> is null.</exception>
    public static IEnumerable<Type> InAssemblies(Assembly[] assemblies, Func<Type, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(assemblies);
        ArgumentNullException.ThrowIfNull(predicate);
        return assemblies.SelectMany(a => a.GetExportedTypes()).Distinct().Where(predicate);
    }

    /// <summary>
    /// Returns all loadable types from any of the specified assemblies, safely handling
    /// <see cref="ReflectionTypeLoadException"/>.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>A distinct enumerable of all loadable types.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assemblies"/> is null.</exception>
    public static IEnumerable<Type> InAssembliesSafe(params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);
        return assemblies.SelectMany(SafeGetExportedTypes).Distinct();
    }

    /// <summary>
    /// Alias for <see cref="InAssembly(Assembly)"/>. Returns all exported types in the assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns>An enumerable of all exported types.</returns>
    public static IEnumerable<Type> In(Assembly assembly) => InAssembly(assembly);

    /// <summary>
    /// Filters an enumerable of types by the specified predicate.
    /// This is an extension method that enables fluent chaining: <c>Types.InAssembly(asm).That(filter)</c>.
    /// </summary>
    /// <param name="types">The source type collection.</param>
    /// <param name="predicate">The filter predicate.</param>
    /// <returns>A filtered enumerable of types.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> or <paramref name="predicate"/> is null.</exception>
    public static IEnumerable<Type> That(this IEnumerable<Type> types, Func<Type, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(types);
        ArgumentNullException.ThrowIfNull(predicate);
        return types.Where(predicate);
    }

    /// <summary>
    /// Safely gets exported types from an assembly, returning an empty array
    /// if the assembly is dynamic or throws on <c>GetExportedTypes()</c>.
    /// </summary>
    private static Type[] SafeGetExportedTypes(Assembly assembly)
    {
        try
        {
            return assembly.IsDynamic ? [] : assembly.GetExportedTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t != null).ToArray()!;
        }
        catch (NotSupportedException)
        {
            // Dynamic assemblies may throw NotSupportedException
            return [];
        }
    }
}
