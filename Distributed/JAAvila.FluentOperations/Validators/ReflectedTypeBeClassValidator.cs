using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is a class (not interface, not value type).
/// </summary>
internal class ReflectedTypeBeClassValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeClassValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeClass";
    string IRuleDescriptor.OperationName => "BeClass";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type.IsClass)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to be a class, but '{type.Name}' is {GetTypeKind(type)}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());

    private static string GetTypeKind(Type t) =>
        t.IsInterface
            ? "an interface"
            : t.IsEnum
                ? "an enum"
                : t.IsValueType
                    ? "a value type"
                    : "not a class";
}
