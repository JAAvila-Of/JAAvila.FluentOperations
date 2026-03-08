using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value does not equal the expected value.
/// </summary>
internal class NullableDecimalNotBeValidator(PrincipalChain<decimal?> chain, decimal? expected)
    : IValidator
{
    public static NullableDecimalNotBeValidator New(
        PrincipalChain<decimal?> chain,
        decimal? expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be {0}, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
