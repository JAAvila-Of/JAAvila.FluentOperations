using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is NaN.
/// </summary>
internal class FloatBeNaNValidator(PrincipalChain<float> chain) : IValidator
{
    public static FloatBeNaNValidator New(PrincipalChain<float> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (float.IsNaN(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be NaN, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
