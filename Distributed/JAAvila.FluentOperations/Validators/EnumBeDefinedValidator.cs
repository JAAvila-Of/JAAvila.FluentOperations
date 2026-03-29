using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the enum value is a defined member of the enumeration.
/// </summary>
internal class EnumBeDefinedValidator<T>(PrincipalChain<T> chain) : IValidator, IRuleDescriptor
    where T : Enum
{
    public static EnumBeDefinedValidator<T> New(PrincipalChain<T> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Enum.BeDefined";
    string IRuleDescriptor.OperationName => "BeDefined";
    Type IRuleDescriptor.SubjectType => typeof(T);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (Enum.IsDefined(typeof(T), chain.GetValue()))
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
