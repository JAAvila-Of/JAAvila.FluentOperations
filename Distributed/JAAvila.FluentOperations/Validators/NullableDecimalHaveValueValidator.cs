using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal has a value (is not null).
/// </summary>
internal class NullableDecimalHaveValueValidator(PrincipalChain<decimal?> chain) : IValidator
{
    public static NullableDecimalHaveValueValidator New(PrincipalChain<decimal?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDecimal.HaveValue";

    public bool Validate()
    {
        if (chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The subject was expected to have a value.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
