using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is zero.
/// </summary>
internal class NullableFloatBeZeroValidator(PrincipalChain<float?> chain) : IValidator
{
    public static NullableFloatBeZeroValidator New(PrincipalChain<float?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableFloat.BeZero";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value == 0f)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be zero, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
