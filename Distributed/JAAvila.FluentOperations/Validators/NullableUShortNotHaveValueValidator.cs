using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ushort does not have a value (is null).
/// </summary>
internal class NullableUShortNotHaveValueValidator(PrincipalChain<ushort?> chain) : IValidator
{
    public static NullableUShortNotHaveValueValidator New(PrincipalChain<ushort?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The subject was expected not to have a value.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
