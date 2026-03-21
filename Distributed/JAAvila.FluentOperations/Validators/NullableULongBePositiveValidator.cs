using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ulong value is strictly positive.
/// </summary>
internal class NullableULongBePositiveValidator(PrincipalChain<ulong?> chain) : IValidator
{
    public static NullableULongBePositiveValidator New(PrincipalChain<ulong?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableULong.BePositive";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value > 0UL)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be positive, but a non-positive value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
