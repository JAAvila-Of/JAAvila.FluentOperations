using JAAvila.FluentOperations.Architecture;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type does NOT have a dependency on the specified type (by fully qualified name).
/// </summary>
internal class ReflectedTypeNotHaveDependencyOnTypeValidator(
    PrincipalChain<Type?> chain,
    string fullyQualifiedTypeName
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeNotHaveDependencyOnTypeValidator New(
        PrincipalChain<Type?> chain,
        string fullyQualifiedTypeName
    ) => new(chain, fullyQualifiedTypeName);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotHaveDependencyOnType";
    string IRuleDescriptor.OperationName => "NotHaveDependencyOnType";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["typeName"] = fullyQualifiedTypeName };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (!DependencyScannerProvider.Current.HasDependencyOnType(type, fullyQualifiedTypeName))
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to NOT have a dependency on type '{fullyQualifiedTypeName}', "
            + $"but '{type.Name}' references that type.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
