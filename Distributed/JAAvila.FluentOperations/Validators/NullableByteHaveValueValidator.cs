using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable byte has a value (is not null).
/// </summary>
internal class NullableByteHaveValueValidator(PrincipalChain<byte?> chain) : IValidator
{
    public static NullableByteHaveValueValidator New(PrincipalChain<byte?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableByte.HaveValue";

    public bool Validate()
    {
        if (chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The subject was expected to have a value.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
