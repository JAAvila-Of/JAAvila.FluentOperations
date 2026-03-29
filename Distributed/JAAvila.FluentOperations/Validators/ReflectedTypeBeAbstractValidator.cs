using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is abstract (and not an interface).
/// </summary>
internal class ReflectedTypeBeAbstractValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeAbstractValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeAbstract";
    string IRuleDescriptor.OperationName => "BeAbstract";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        // Abstract and NOT an interface (interfaces are always abstract in CLR)
        if (type is { IsAbstract: true, IsInterface: false })
        {
            return true;
        }

        ResultValidation = type.IsInterface
            ? $"The type was expected to be abstract, but '{type.Name}' is an interface (use BeInterface instead)."
            : $"The type was expected to be abstract, but '{type.Name}' is not abstract.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
