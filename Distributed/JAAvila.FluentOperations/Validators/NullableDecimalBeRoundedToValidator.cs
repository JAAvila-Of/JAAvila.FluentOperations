using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value equals itself when rounded to the specified decimal places.
/// </summary>
internal class NullableDecimalBeRoundedToValidator(PrincipalChain<decimal?> chain, int decimals)
    : IValidator
{
    public static NullableDecimalBeRoundedToValidator New(
        PrincipalChain<decimal?> chain,
        int decimals
    ) => new(chain, decimals);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDecimal.BeRoundedTo";

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (Math.Round(value, decimals) == value)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be rounded to {0} decimal places, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
