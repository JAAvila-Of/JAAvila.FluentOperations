using System.Reflection;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type HAS at least one async void method.
/// Primarily used as the positive counterpart of <see cref="ReflectedTypeNotHaveAsyncVoidMethodsValidator"/>.
/// </summary>
internal class ReflectedTypeHaveAsyncVoidMethodsValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeHaveAsyncVoidMethodsValidator New(PrincipalChain<Type?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveAsyncVoidMethods";
    string IRuleDescriptor.OperationName => "HaveAsyncVoidMethods";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var hasAsyncVoid = type.GetMethods(
                BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.DeclaredOnly
            )
            .Any(
                m =>
                    m.ReturnType == typeof(void)
                    && m.GetCustomAttribute<AsyncStateMachineAttribute>() != null
            );

        if (hasAsyncVoid)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to have at least one async void method, "
            + $"but '{type.Name}' has none.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
