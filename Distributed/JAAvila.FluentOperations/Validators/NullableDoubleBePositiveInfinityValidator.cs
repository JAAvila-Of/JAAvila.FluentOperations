using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value is positive infinity.
/// </summary>
internal class NullableDoubleBePositiveInfinityValidator(PrincipalChain<double?> chain) : IValidator
{
    public static NullableDoubleBePositiveInfinityValidator New(PrincipalChain<double?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDouble.BePositiveInfinity";

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (double.IsPositiveInfinity(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be positive infinity, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
