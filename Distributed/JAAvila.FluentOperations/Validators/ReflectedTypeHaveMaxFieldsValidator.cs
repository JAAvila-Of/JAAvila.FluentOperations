using System.Reflection;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type has at most the specified number of fields
/// (instance + static, all visibilities, declared only, excluding compiler-generated backing fields).
/// </summary>
internal class ReflectedTypeHaveMaxFieldsValidator(PrincipalChain<Type?> chain, int maxCount)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeHaveMaxFieldsValidator New(
        PrincipalChain<Type?> chain,
        int maxCount
    ) => new(chain, maxCount);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.HaveMaxFields";
    string IRuleDescriptor.OperationName => "HaveMaxFields";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["maxCount"] = maxCount };

    public bool Validate()
    {
        var type = chain.GetValue()!;
        var count = type.GetFields(
                BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.DeclaredOnly
            )
            .Count(f => !f.Name.StartsWith('<')); // Exclude compiler-generated backing fields

        if (count <= maxCount)
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to have at most {maxCount} fields, "
            + $"but '{type.Name}' has {count}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
