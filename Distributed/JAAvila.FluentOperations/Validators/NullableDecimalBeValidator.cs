using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value equals the expected value.
/// </summary>
internal class NullableDecimalBeValidator(PrincipalChain<decimal?> chain, decimal? expected)
    : IValidator
{
    public static NullableDecimalBeValidator New(
        PrincipalChain<decimal?> chain,
        decimal? expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDecimal.Be";

    public bool Validate()
    {
        if (chain.GetValue() == expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
