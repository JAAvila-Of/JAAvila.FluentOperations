using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is NOT in the specified namespace.
/// </summary>
internal class ReflectedTypeNotBeInNamespaceValidator(
    PrincipalChain<Type?> chain,
    string expectedNamespace
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeNotBeInNamespaceValidator New(
        PrincipalChain<Type?> chain,
        string expectedNamespace
    ) => new(chain, expectedNamespace);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotBeInNamespace";
    string IRuleDescriptor.OperationName => "NotBeInNamespace";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["expectedNamespace"] = expectedNamespace };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (!string.Equals(type.Namespace, expectedNamespace, StringComparison.Ordinal))
        {
            return true;
        }

        ResultValidation = "The type was expected to NOT be in namespace \"{0}\", but it is.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
