using System.Reflection;
using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type does NOT have any protected (or protected internal) members.
/// </summary>
internal class ReflectedTypeNotHaveProtectedMembersValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotHaveProtectedMembersValidator New(PrincipalChain<Type?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotHaveProtectedMembers";
    string IRuleDescriptor.OperationName => "NotHaveProtectedMembers";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    private const BindingFlags DeclaredMembers =
        BindingFlags.Public
        | BindingFlags.NonPublic
        | BindingFlags.Instance
        | BindingFlags.Static
        | BindingFlags.DeclaredOnly;

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var protectedMembers = (
            from field in type.GetFields(DeclaredMembers)
            where field.IsFamily || field.IsFamilyOrAssembly
            select $"field '{field.Name}'"
        ).ToList();
        protectedMembers.AddRange(
            from method in type.GetMethods(DeclaredMembers)
            where method.IsFamily || method.IsFamilyOrAssembly
            select $"method '{method.Name}'"
        );

        protectedMembers.AddRange(
            from prop in type.GetProperties(DeclaredMembers)
            let getter = prop.GetGetMethod(true)
            let setter = prop.GetSetMethod(true)
            where
                (getter != null && (getter.IsFamily || getter.IsFamilyOrAssembly))
                || (setter != null && (setter.IsFamily || setter.IsFamilyOrAssembly))
            select $"property '{prop.Name}'"
        );

        protectedMembers.AddRange(
            from adder in from evt in type.GetEvents(DeclaredMembers)
            let adder = evt.GetAddMethod(true)
            select adder
            where !adder.IsNull() && (adder.IsFamily || adder.IsFamilyOrAssembly)
            select $"event '{adder.Name}'"
        );

        if (protectedMembers.Count == 0)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to have no protected members, "
            + $"but '{type.Name}' has {protectedMembers.Count}: [{string.Join(", ", protectedMembers)}].";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
