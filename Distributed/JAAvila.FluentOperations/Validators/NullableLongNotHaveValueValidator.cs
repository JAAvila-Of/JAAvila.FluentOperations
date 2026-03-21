using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable long does not have a value (is null).
/// </summary>
internal class NullableLongNotHaveValueValidator(PrincipalChain<long?> chain) : IValidator
{
    public static NullableLongNotHaveValueValidator New(PrincipalChain<long?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableLong.NotHaveValue";

    public bool Validate()
    {
        if (!chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The subject was expected not to have a value, but a value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
