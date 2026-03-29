using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type HAS at least one public method that returns a type from the specified namespace.
/// </summary>
internal class ReflectedTypeReturnTypesFromNamespaceValidator(
    PrincipalChain<Type?> chain,
    string namespacePrefix
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeReturnTypesFromNamespaceValidator New(
        PrincipalChain<Type?> chain,
        string namespacePrefix
    ) => new(chain, namespacePrefix);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.ReturnTypesFromNamespace";
    string IRuleDescriptor.OperationName => "ReturnTypesFromNamespace";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["namespace"] = namespacePrefix };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (
            type.GetMethods(
                    BindingFlags.Public
                        | BindingFlags.Instance
                        | BindingFlags.Static
                        | BindingFlags.DeclaredOnly
                )
                .Where(method => !method.IsSpecialName)
                .Any(method => TypeMatchesNamespace(method.ReturnType, namespacePrefix))
        )
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to return types from namespace '{namespacePrefix}', "
            + $"but '{type.Name}' has no public methods returning types from that namespace.";
        return false;
    }

    private static bool TypeMatchesNamespace(Type returnType, string ns)
    {
        if (
            returnType.Namespace != null
            && (
                string.Equals(returnType.Namespace, ns, StringComparison.Ordinal)
                || returnType.Namespace.StartsWith(ns + ".", StringComparison.Ordinal)
            )
        )
            return true;

        if (returnType.IsGenericType)
        {
            foreach (var arg in returnType.GetGenericArguments())
            {
                if (!arg.IsGenericParameter && TypeMatchesNamespace(arg, ns))
                    return true;
            }
        }

        if (returnType.IsArray && returnType.GetElementType() is { } elemType)
            return TypeMatchesNamespace(elemType, ns);

        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
