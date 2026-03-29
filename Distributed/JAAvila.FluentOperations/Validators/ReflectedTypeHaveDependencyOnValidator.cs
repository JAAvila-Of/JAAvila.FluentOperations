using JAAvila.FluentOperations.Architecture;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has a dependency on the specified namespace.
/// </summary>
internal class ReflectedTypeHaveDependencyOnValidator(
    PrincipalChain<Type?> chain,
    string namespacePrefix
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeHaveDependencyOnValidator New(
        PrincipalChain<Type?> chain,
        string namespacePrefix
    ) => new(chain, namespacePrefix);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveDependencyOn";
    string IRuleDescriptor.OperationName => "HaveDependencyOn";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["namespace"] = namespacePrefix };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (DependencyScannerProvider.Current.HasDependencyOnNamespace(type, namespacePrefix))
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to have a dependency on '{namespacePrefix}', "
            + $"but '{type.Name}' does not reference that namespace.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
