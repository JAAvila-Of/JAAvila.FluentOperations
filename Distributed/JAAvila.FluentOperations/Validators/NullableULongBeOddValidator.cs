using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ulong value is odd.
/// </summary>
internal class NullableULongBeOddValidator(PrincipalChain<ulong?> chain) : IValidator
{
    public static NullableULongBeOddValidator New(PrincipalChain<ulong?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableULong.BeOdd";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value % 2UL != 0UL)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be odd, but an even value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
