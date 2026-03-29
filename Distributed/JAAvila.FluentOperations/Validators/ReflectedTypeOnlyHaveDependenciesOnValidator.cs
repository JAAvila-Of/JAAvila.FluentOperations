using JAAvila.FluentOperations.Architecture;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type only has dependencies on the specified namespaces (whitelist).
/// </summary>
internal class ReflectedTypeOnlyHaveDependenciesOnValidator(
    PrincipalChain<Type?> chain,
    string[] allowedNamespaces
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeOnlyHaveDependenciesOnValidator New(
        PrincipalChain<Type?> chain,
        params string[] allowedNamespaces
    ) => new(chain, allowedNamespaces);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.OnlyHaveDependenciesOn";
    string IRuleDescriptor.OperationName => "OnlyHaveDependenciesOn";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["allowedNamespaces"] = allowedNamespaces };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var (isCompliant, violating) = DependencyScannerProvider.Current.CheckOnlyDependenciesOn(
            type,
            allowedNamespaces
        );

        if (isCompliant)
        {
            return true;
        }

        var allowedStr = string.Join(", ", allowedNamespaces);
        var violatingStr = string.Join(", ", violating);
        ResultValidation =
            $"The type was expected to only have dependencies on [{allowedStr}], "
            + $"but '{type.Name}' also depends on: [{violatingStr}].";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
