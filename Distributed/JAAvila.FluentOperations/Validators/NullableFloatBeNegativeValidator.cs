using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is strictly negative.
/// </summary>
internal class NullableFloatBeNegativeValidator(PrincipalChain<float?> chain) : IValidator
{
    public static NullableFloatBeNegativeValidator New(PrincipalChain<float?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableFloat.BeNegative";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < 0)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be negative, but a non-negative value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
