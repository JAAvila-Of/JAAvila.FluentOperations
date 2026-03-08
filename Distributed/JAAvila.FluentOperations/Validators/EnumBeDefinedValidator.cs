using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the enum value is a defined member of the enumeration.
/// </summary>
internal class EnumBeDefinedValidator<T>(PrincipalChain<T> chain) : IValidator
    where T : Enum
{
    public static EnumBeDefinedValidator<T> New(PrincipalChain<T> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
