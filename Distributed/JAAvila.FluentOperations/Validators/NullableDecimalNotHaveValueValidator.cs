using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal does not have a value (is null).
/// </summary>
internal class NullableDecimalNotHaveValueValidator(PrincipalChain<decimal?> chain) : IValidator
{
    public static NullableDecimalNotHaveValueValidator New(PrincipalChain<decimal?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDecimal.NotHaveValue";

    public bool Validate()
    {
        if (!chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The subject was expected not to have a value, but a value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
