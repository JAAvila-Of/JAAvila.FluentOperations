using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable decimal value is zero.
/// </summary>
internal class NullableDecimalBeZeroValidator(PrincipalChain<decimal?> chain) : IValidator
{
    public static NullableDecimalBeZeroValidator New(PrincipalChain<decimal?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value == 0m)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be zero, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
