using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that none of the type's public methods return types from the specified namespace.
/// Also inspects generic type arguments of return types (e.g., <c>Task&lt;DbConnection&gt;</c>).
/// </summary>
internal class ReflectedTypeNotReturnTypesFromNamespaceValidator(
    PrincipalChain<Type?> chain,
    string namespacePrefix
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeNotReturnTypesFromNamespaceValidator New(
        PrincipalChain<Type?> chain,
        string namespacePrefix
    ) => new(chain, namespacePrefix);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotReturnTypesFromNamespace";
    string IRuleDescriptor.OperationName => "NotReturnTypesFromNamespace";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["namespace"] = namespacePrefix };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var violations = (
            from method in type.GetMethods(
                BindingFlags.Public
                    | BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.DeclaredOnly
            )
            where !method.IsSpecialName
            where TypeMatchesNamespace(method.ReturnType, namespacePrefix)
            select $"'{method.Name}' returns '{method.ReturnType.Name}'"
        ).ToList();

        if (violations.Count == 0)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to NOT return types from namespace '{namespacePrefix}', "
            + $"but '{type.Name}' has violations: [{string.Join(", ", violations)}].";
        return false;
    }

    /// <summary>
    /// Recursively checks if a type or any of its generic arguments belongs to the namespace.
    /// </summary>
    private static bool TypeMatchesNamespace(Type returnType, string ns)
    {
        // Direct match
        if (
            returnType.Namespace != null
            && (
                string.Equals(returnType.Namespace, ns, StringComparison.Ordinal)
                || returnType.Namespace.StartsWith(ns + ".", StringComparison.Ordinal)
            )
        )
        {
            return true;
        }

        // Check generic arguments (e.g., Task<DbConnection>, IEnumerable<EfEntity>)
        if (returnType.IsGenericType)
        {
            if (
                returnType
                    .GetGenericArguments()
                    .Any(arg => !arg.IsGenericParameter && TypeMatchesNamespace(arg, ns))
            )
            {
                return true;
            }
        }

        // Check array element type
        if (returnType.IsArray && returnType.GetElementType() is { } elemType)
        {
            return TypeMatchesNamespace(elemType, ns);
        }

        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
