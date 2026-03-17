using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint value is strictly positive.
/// </summary>
internal class NullableUIntBePositiveValidator(PrincipalChain<uint?> chain) : IValidator
{
    public static NullableUIntBePositiveValidator New(PrincipalChain<uint?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value > 0)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be positive, but a non-positive value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
