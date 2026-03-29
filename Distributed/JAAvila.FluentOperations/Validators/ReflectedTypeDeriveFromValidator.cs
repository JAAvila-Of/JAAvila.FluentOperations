using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type derives from the specified base type.
/// </summary>
internal class ReflectedTypeDeriveFromValidator(PrincipalChain<Type?> chain, Type expectedBase)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeDeriveFromValidator New(
        PrincipalChain<Type?> chain,
        Type expectedBase
    ) => new(chain, expectedBase);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.DeriveFrom";
    string IRuleDescriptor.OperationName => "DeriveFrom";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["baseType"] = expectedBase };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Support open generics: typeof(List<>) derives from typeof(IEnumerable<>)
        if (expectedBase.IsGenericTypeDefinition)
        {
            var current = type.BaseType;

            while (current != null)
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == expectedBase)
                {
                    return true;
                }

                current = current.BaseType;
            }
        }
        else
        {
            if (expectedBase.IsAssignableFrom(type) && type != expectedBase)
            {
                return true;
            }
        }

        ResultValidation =
            "The type was expected to derive from '{0}', but '{1}' does not derive from it.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
