using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type does NOT implement the specified interface.
/// </summary>
internal class ReflectedTypeNotImplementInterfaceValidator(
    PrincipalChain<Type?> chain,
    Type expectedInterface
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeNotImplementInterfaceValidator New(
        PrincipalChain<Type?> chain,
        Type expectedInterface
    ) => new(chain, expectedInterface);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotImplementInterface";
    string IRuleDescriptor.OperationName => "NotImplementInterface";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["interface"] = expectedInterface };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Support open generics: typeof(IRepository<>) should match IRepository<Customer>
        bool implements;

        if (expectedInterface.IsGenericTypeDefinition)
        {
            implements = type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == expectedInterface);
        }
        else
        {
            implements = expectedInterface.IsAssignableFrom(type);
        }

        if (!implements)
        {
            return true;
        }

        ResultValidation =
            "The type was expected to NOT implement '{0}', but '{1}' does implement it.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
