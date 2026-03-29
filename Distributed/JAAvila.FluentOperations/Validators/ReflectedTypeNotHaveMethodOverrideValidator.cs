using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type does NOT override the specified method.
/// </summary>
internal class ReflectedTypeNotHaveMethodOverrideValidator(
    PrincipalChain<Type?> chain,
    string methodName
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeNotHaveMethodOverrideValidator New(
        PrincipalChain<Type?> chain,
        string methodName
    ) => new(chain, methodName);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotHaveMethodOverride";
    string IRuleDescriptor.OperationName => "NotHaveMethodOverride";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["methodName"] = methodName };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var methods = type.GetMethods(
            BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Instance
                | BindingFlags.DeclaredOnly
        );

        foreach (var method in methods)
        {
            if (method.Name != methodName)
            {
                continue;
            }

            var baseDefinition = method.GetBaseDefinition();

            if (baseDefinition.DeclaringType != type)
            {
                ResultValidation =
                    $"The type was expected to NOT override method '{methodName}', "
                    + $"but '{type.Name}' overrides it (base: '{baseDefinition.DeclaringType!.Name}').";
                return false;
            }
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
