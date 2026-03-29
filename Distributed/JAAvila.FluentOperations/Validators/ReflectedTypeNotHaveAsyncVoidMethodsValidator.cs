using System.Reflection;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type does NOT have any async void methods.
/// An async void method is a method with return type <c>void</c> that carries
/// <see cref="AsyncStateMachineAttribute"/>.
/// </summary>
internal class ReflectedTypeNotHaveAsyncVoidMethodsValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeNotHaveAsyncVoidMethodsValidator New(PrincipalChain<Type?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.NotHaveAsyncVoidMethods";
    string IRuleDescriptor.OperationName => "NotHaveAsyncVoidMethods";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var asyncVoidMethods = type.GetMethods(
                BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.DeclaredOnly
            )
            .Where(
                m =>
                    m.ReturnType == typeof(void)
                    && m.GetCustomAttribute<AsyncStateMachineAttribute>() != null
            )
            .ToList();

        if (asyncVoidMethods.Count == 0)
        {
            return true;
        }

        var names = string.Join(", ", asyncVoidMethods.Select(m => m.Name));
        ResultValidation =
            $"The type was expected to have no async void methods, "
            + $"but '{type.Name}' has {asyncVoidMethods.Count}: [{names}].";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
