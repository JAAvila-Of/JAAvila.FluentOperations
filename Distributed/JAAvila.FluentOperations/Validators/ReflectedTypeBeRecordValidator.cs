using System.Text;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the type is a record (class or struct).
/// </summary>
internal class ReflectedTypeBeRecordValidator(PrincipalChain<Type?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReflectedTypeBeRecordValidator New(PrincipalChain<Type?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ReflectedType.BeRecord";
    string IRuleDescriptor.OperationName => "BeRecord";
    Type IRuleDescriptor.SubjectType => typeof(Type);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var type = chain.GetValue()!;

        if (IsRecord(type))
        {
            return true;
        }

        ResultValidation =
            $"The type was expected to be a record, but '{type.Name}' is not a record.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());

    private static bool IsRecord(Type type)
    {
        if (type.IsClass)
        {
            var hasClone =
                type.GetMethod(
                    "<Clone>$",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
                ) != null;
            var hasEqualityContract =
                type.GetProperty(
                    "EqualityContract",
                    System.Reflection.BindingFlags.NonPublic
                        | System.Reflection.BindingFlags.Instance
                ) != null;
            return hasClone && hasEqualityContract;
        }

        if (type is { IsValueType: true, IsEnum: false })
        {
            var hasPrintMembers =
                type.GetMethod(
                    "PrintMembers",
                    System.Reflection.BindingFlags.NonPublic
                        | System.Reflection.BindingFlags.Instance,
                    null,
                    [typeof(StringBuilder)],
                    null
                ) != null;

            if (!hasPrintMembers)
            {
                return false;
            }

            var equatableInterface = typeof(IEquatable<>).MakeGenericType(type);
            return equatableInterface.IsAssignableFrom(type);
        }

        return false;
    }
}
