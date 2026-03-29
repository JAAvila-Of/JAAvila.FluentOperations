using JAAvila.FluentOperations.Architecture;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has a dependency on at least one of the specified namespaces.
/// </summary>
internal class ReflectedTypeHaveDependencyOnAnyValidator(
    PrincipalChain<Type?> chain,
    string[] namespacePrefixes
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeHaveDependencyOnAnyValidator New(
        PrincipalChain<Type?> chain,
        string[] namespacePrefixes
    ) => new(chain, namespacePrefixes);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveDependencyOnAny";
    string IRuleDescriptor.OperationName => "HaveDependencyOnAny";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["namespaces"] = namespacePrefixes };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var scanner = DependencyScannerProvider.Current;

        if (namespacePrefixes.Any(ns => scanner.HasDependencyOnNamespace(type, ns)))
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to have a dependency on at least one of "
            + $"[{string.Join(", ", namespacePrefixes.Select(n => $"'{n}'"))}], "
            + $"but '{type.Name}' does not reference any of them.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
