using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is an interface.
/// </summary>
internal class ReflectedTypeBeInterfaceValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeInterfaceValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeInterface";
    string IRuleDescriptor.OperationName => "BeInterface";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (type.IsInterface)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to be an interface, but '{type.Name}' is {(type.IsClass ? "a class" : type.IsValueType ? "a value type" : "not an interface")}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
