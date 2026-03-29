using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type resides in the specified namespace.
/// </summary>
internal class ReflectedTypeBeInNamespaceValidator(
    PrincipalChain<Type?> chain,
    string expectedNamespace
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeBeInNamespaceValidator New(
        PrincipalChain<Type?> chain,
        string expectedNamespace
    ) => new(chain, expectedNamespace);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeInNamespace";
    string IRuleDescriptor.OperationName => "BeInNamespace";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["namespace"] = expectedNamespace };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (string.Equals(type.Namespace, expectedNamespace, StringComparison.Ordinal))
        {
            return true;
        }

        ResultValidation =
            "The type was expected to be in namespace \"{0}\", but it is in \"{1}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
