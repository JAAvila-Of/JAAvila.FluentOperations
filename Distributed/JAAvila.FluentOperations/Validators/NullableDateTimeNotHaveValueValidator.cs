using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime does not have a value (is null).
/// </summary>
internal class NullableDateTimeNotHaveValueValidator(PrincipalChain<DateTime?> chain) : IValidator
{
    public static NullableDateTimeNotHaveValueValidator New(PrincipalChain<DateTime?> chain) =>
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
