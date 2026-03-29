using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type HAS at least one protected (or protected internal) member.
/// </summary>
internal class ReflectedTypeHaveProtectedMembersValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeHaveProtectedMembersValidator New(PrincipalChain<Type?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveProtectedMembers";
    string IRuleDescriptor.OperationName => "HaveProtectedMembers";
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

        // Check fields
        if (type.GetFields(DeclaredMembers).Any(f => f.IsFamily || f.IsFamilyOrAssembly))
        {
            return true;
        }

        // Check methods
        if (type.GetMethods(DeclaredMembers).Any(m => m.IsFamily || m.IsFamilyOrAssembly))
        {
            return true;
        }

        // Check properties (via accessor visibility)
        if (
            (
                from prop in type.GetProperties(DeclaredMembers)
                let getter = prop.GetGetMethod(true)
                let setter = prop.GetSetMethod(true)
                where
                    (getter != null && (getter.IsFamily || getter.IsFamilyOrAssembly))
                    || (setter != null && (setter.IsFamily || setter.IsFamilyOrAssembly))
                select getter
            ).Any()
        )
        {
            return true;
        }

        // Check events
        if (
            type.GetEvents(DeclaredMembers)
                .Select(evt => evt.GetAddMethod(true))
                .OfType<MethodInfo>()
                .Any(adder => adder.IsFamily || adder.IsFamilyOrAssembly)
        )
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to have at least one protected member, "
            + $"but '{type.Name}' has none.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
