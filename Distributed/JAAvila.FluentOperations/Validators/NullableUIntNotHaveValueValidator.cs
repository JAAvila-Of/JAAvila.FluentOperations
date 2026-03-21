using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable uint does not have a value (is null).
/// </summary>
internal class NullableUIntNotHaveValueValidator(PrincipalChain<uint?> chain) : IValidator
{
    public static NullableUIntNotHaveValueValidator New(PrincipalChain<uint?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableUInt.NotHaveValue";

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
