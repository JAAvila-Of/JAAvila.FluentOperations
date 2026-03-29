using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type resides in a namespace starting with the specified prefix.
/// </summary>
internal class ReflectedTypeBeInNamespacePrefixValidator(PrincipalChain<Type?> chain, string prefix)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeInNamespacePrefixValidator New(
        PrincipalChain<Type?> chain,
        string prefix
    ) => new(chain, prefix);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeInNamespaceStartingWith";
    string IRuleDescriptor.OperationName => "BeInNamespaceStartingWith";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["prefix"] = prefix };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var ns = type.Namespace;

        if (
            ns != null
            && (
                ns.Equals(prefix, StringComparison.Ordinal)
                || ns.StartsWith(prefix + ".", StringComparison.Ordinal)
            )
        )
        {
            return true;
        }

        ResultValidation =
            "The type was expected to be in a namespace starting with \"{0}\", but it is in \"{1}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
