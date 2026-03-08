using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value is evenly divisible by the specified divisor.
/// </summary>
internal class FloatBeDivisibleByValidator(PrincipalChain<float> chain, float divisor) : IValidator
{
    public static FloatBeDivisibleByValidator New(PrincipalChain<float> chain, float divisor) =>
        new(chain, divisor);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Math.Abs(value % divisor) < 1e-6f)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be divisible by {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
