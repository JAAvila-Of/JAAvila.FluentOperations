using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has a public method with the specified name and return type.
/// </summary>
internal class ReflectedTypeHaveMethodReturningValidator(
    PrincipalChain<Type?> chain,
    string methodName,
    Type expectedReturnType
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeHaveMethodReturningValidator New(
        PrincipalChain<Type?> chain,
        string methodName,
        Type expectedReturnType
    ) => new(chain, methodName, expectedReturnType);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveMethodReturning";
    string IRuleDescriptor.OperationName => "HaveMethodReturning";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>
        {
            ["methodName"] = methodName,
            ["returnType"] = expectedReturnType
        };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var methods = type.GetMethods(
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
        );
        var match = methods.FirstOrDefault(
            m => m.Name == methodName && m.ReturnType == expectedReturnType
        );

        if (match != null)
        {
            return true;
        }

        var existing = methods.FirstOrDefault(m => m.Name == methodName);

        if (existing == null)
        {
            ResultValidation =
                $"The type was expected to have a method '{methodName}' "
                + $"returning '{expectedReturnType.Name}', but '{type.Name}' has no method named '{methodName}'.";
        }
        else
        {
            ResultValidation =
                $"The type was expected to have a method '{methodName}' "
                + $"returning '{expectedReturnType.Name}', but the actual return type is '{existing.ReturnType.Name}'.";
        }

        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
