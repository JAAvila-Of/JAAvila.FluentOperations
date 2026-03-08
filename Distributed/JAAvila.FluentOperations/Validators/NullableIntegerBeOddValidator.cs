using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is odd.
/// </summary>
internal class NullableIntegerBeOddValidator(PrincipalChain<int?> chain) : IValidator
{
    public static NullableIntegerBeOddValidator New(PrincipalChain<int?> chain) => new(chain);

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
