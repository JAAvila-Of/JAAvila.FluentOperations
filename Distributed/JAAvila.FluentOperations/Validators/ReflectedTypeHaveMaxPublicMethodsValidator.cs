using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has at most the specified number of public methods (declared only, excluding
/// property accessors, event accessors, and methods inherited from <see cref="object"/>).
/// </summary>
internal class ReflectedTypeHaveMaxPublicMethodsValidator(PrincipalChain<Type?> chain, int maxCount)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeHaveMaxPublicMethodsValidator New(
        PrincipalChain<Type?> chain,
        int maxCount
    ) => new(chain, maxCount);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveMaxPublicMethods";
    string IRuleDescriptor.OperationName => "HaveMaxPublicMethods";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["maxCount"] = maxCount };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var count = type.GetMethods(
                BindingFlags.Public
                    | BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.DeclaredOnly
            )
            .Count(m => !m.IsSpecialName); // Excludes property/event accessors, operators

        if (count <= maxCount)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to have at most {maxCount} public methods, "
            + $"but '{type.Name}' has {count}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
