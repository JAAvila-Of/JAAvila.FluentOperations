using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type overrides the specified method (declared in a base type).
/// </summary>
internal class ReflectedTypeHaveMethodOverrideValidator(
    PrincipalChain<Type?> chain,
    string methodName
) : IValidator, IRuleDescriptor
{
    public static ReflectedTypeHaveMethodOverrideValidator New(
        PrincipalChain<Type?> chain,
        string methodName
    ) => new(chain, methodName);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveMethodOverride";
    string IRuleDescriptor.OperationName => "HaveMethodOverride";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["methodName"] = methodName };

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Search instance methods (overrides are always instance, not static)
        var methods = type.GetMethods(
            BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Instance
                | BindingFlags.DeclaredOnly
        );

        if (
            (
                from method in methods
                where method.Name == methodName
                select method.GetBaseDefinition()
            ).Any(baseDefinition => baseDefinition.DeclaringType != type)
        )
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to override method '{methodName}', "
            + $"but '{type.Name}' does not override it.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
