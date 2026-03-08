using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable long value is odd.
/// </summary>
internal class NullableLongBeOddValidator(PrincipalChain<long?> chain) : IValidator
{
    public static NullableLongBeOddValidator New(PrincipalChain<long?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value % 2 != 0)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be odd, but an even value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
