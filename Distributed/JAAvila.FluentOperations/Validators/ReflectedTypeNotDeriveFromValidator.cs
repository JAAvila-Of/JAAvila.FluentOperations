using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type does NOT derive from the specified base type.
/// </summary>
internal class ReflectedTypeNotDeriveFromValidator(PrincipalChain<Type?> chain, Type expectedBase)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotDeriveFromValidator New(
        PrincipalChain<Type?> chain,
        Type expectedBase
    ) => new(chain, expectedBase);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotDeriveFrom";
    string IRuleDescriptor.OperationName => "NotDeriveFrom";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["baseType"] = expectedBase };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Support open generics
        bool derives;

        if (expectedBase.IsGenericTypeDefinition)
        {
            var current = type.BaseType;
            derives = false;

            while (current != null)
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == expectedBase)
                {
                    derives = true;
                    break;
                }

                current = current.BaseType;
            }
        }
        else
        {
            derives = expectedBase.IsAssignableFrom(type) && type != expectedBase;
        }

        if (!derives)
        {
            return true;
        }

        ResultValidation =
            "The type was expected to NOT derive from '{0}', but '{1}' does derive from it.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
