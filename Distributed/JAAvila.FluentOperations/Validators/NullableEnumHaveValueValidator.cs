using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable enum has a value (is not null).
/// </summary>
internal class NullableEnumHaveValueValidator<T>(PrincipalChain<T?> chain) : IValidator
    where T : struct, Enum
{
    public static NullableEnumHaveValueValidator<T> New(PrincipalChain<T?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableEnum.HaveValue";

    public bool Validate()
    {
        if (chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have a value, but it was <null>.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
