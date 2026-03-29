using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type implements the specified interface.
/// </summary>
internal class ReflectedTypeImplementInterfaceValidator(
    PrincipalChain<Type?> chain,
    Type expectedInterface
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeImplementInterfaceValidator New(
        PrincipalChain<Type?> chain,
        Type expectedInterface
    ) => new(chain, expectedInterface);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.ImplementInterface";
    string IRuleDescriptor.OperationName => "ImplementInterface";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["interface"] = expectedInterface };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Support open generics: typeof(IRepository<>) should match IRepository<Customer>
        if (expectedInterface.IsGenericTypeDefinition)
        {
            if (
                type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == expectedInterface)
            )
            {
                return true;
            }
        }
        else
        {
            if (expectedInterface.IsAssignableFrom(type))
            {
                return true;
            }
        }

        ResultValidation =
            "The type was expected to implement '{0}', but '{1}' does not implement it.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
