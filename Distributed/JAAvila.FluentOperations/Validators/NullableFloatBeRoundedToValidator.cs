using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value equals itself when rounded to the specified decimal places.
/// </summary>
internal class NullableFloatBeRoundedToValidator(PrincipalChain<float?> chain, int decimals)
    : IValidator
{
    public static NullableFloatBeRoundedToValidator New(
        PrincipalChain<float?> chain,
        int decimals
    ) => new(chain, decimals);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableFloat.BeRoundedTo";

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (Math.Abs((float)Math.Round(value, decimals) - value) < 1e-6f)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be rounded to the given number of decimal places, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
