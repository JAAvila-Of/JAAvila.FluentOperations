using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the byte value is less than or equal to the expected value.
/// </summary>
internal class ByteBeLessThanOrEqualToValidator(PrincipalChain<byte> chain, byte expected) : IValidator
{
    public static ByteBeLessThanOrEqualToValidator New(PrincipalChain<byte> chain, byte expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() <= expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be less than or equal to {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
