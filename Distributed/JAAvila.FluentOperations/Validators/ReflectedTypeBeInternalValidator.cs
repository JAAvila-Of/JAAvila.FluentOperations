using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is internal (not public).
/// </summary>
internal class ReflectedTypeBeInternalValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeInternalValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeInternal";
    string IRuleDescriptor.OperationName => "BeInternal";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Top-level internal: IsNotPublic. Nested internal: IsNestedAssembly or IsNestedFamORAssem.
        if (type.IsNotPublic || type.IsNestedAssembly || type.IsNestedFamORAssem)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to be internal, but '{type.Name}' is not internal.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
