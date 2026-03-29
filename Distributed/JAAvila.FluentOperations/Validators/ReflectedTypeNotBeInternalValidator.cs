using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is NOT internal.
/// </summary>
internal class ReflectedTypeNotBeInternalValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotBeInternalValidator New(PrincipalChain<Type?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotBeInternal";
    string IRuleDescriptor.OperationName => "NotBeInternal";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (!(type.IsNotPublic || type.IsNestedAssembly || type.IsNestedFamORAssem))
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to NOT be internal, but '{type.Name}' is internal.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
