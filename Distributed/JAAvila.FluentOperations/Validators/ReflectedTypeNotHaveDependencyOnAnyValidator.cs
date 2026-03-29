using JAAvila.FluentOperations.Architecture;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type does NOT have a dependency on any of the specified namespaces.
/// </summary>
internal class ReflectedTypeNotHaveDependencyOnAnyValidator(
    PrincipalChain<Type?> chain,
    string[] namespacePrefixes
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeNotHaveDependencyOnAnyValidator New(
        PrincipalChain<Type?> chain,
        string[] namespacePrefixes
    ) => new(chain, namespacePrefixes);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotHaveDependencyOnAny";
    string IRuleDescriptor.OperationName => "NotHaveDependencyOnAny";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["namespaces"] = namespacePrefixes };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var scanner = DependencyScannerProvider.Current;
        var matched = namespacePrefixes
            .Where(ns => scanner.HasDependencyOnNamespace(type, ns))
            .ToList();

        if (matched.Count == 0)
        {
            return true;
        }

        ResultValidation =
            "The type was expected to NOT have a dependency on any of "
            + $"[{string.Join(", ", namespacePrefixes.Select(n => $"'{n}'"))}], "
            + $"but '{type.Name}' references: [{string.Join(", ", matched.Select(n => $"'{n}'"))}].";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
