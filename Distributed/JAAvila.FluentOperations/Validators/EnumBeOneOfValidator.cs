using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the enum value is one of the specified allowed values.
/// </summary>
internal class EnumBeOneOfValidator<T>(PrincipalChain<T> chain, params T[] expected) : IValidator
    where T : Enum
{
    public static EnumBeOneOfValidator<T> New(PrincipalChain<T> chain, params T[] expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (expected.Contains(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be one of [{0}], but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
