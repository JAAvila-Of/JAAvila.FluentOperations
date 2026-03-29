using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable enum value is a defined member of the enumeration.
/// </summary>
internal class NullableEnumBeDefinedValidator<T>(PrincipalChain<T?> chain)
    : IValidator,
        IRuleDescriptor
    where T : struct, Enum
{
    public static NullableEnumBeDefinedValidator<T> New(PrincipalChain<T?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableEnum.BeDefined";
    string IRuleDescriptor.OperationName => "BeDefined";
    Type IRuleDescriptor.SubjectType => typeof(T?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (Enum.IsDefined(typeof(T), chain.GetValue()!.Value))
        {
            return true;
        }

        ResultValidation = "The value {0} is not a defined member of enum {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
