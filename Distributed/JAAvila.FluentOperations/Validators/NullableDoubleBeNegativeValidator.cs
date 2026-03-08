using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is strictly negative.
/// </summary>
internal class NullableDoubleBeNegativeValidator(PrincipalChain<double?> chain) : IValidator
{
    public static NullableDoubleBeNegativeValidator New(PrincipalChain<double?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
