using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timeonly does not have a value (is null).
/// </summary>
internal class NullableTimeOnlyNotHaveValueValidator(PrincipalChain<TimeOnly?> chain) : IValidator
{
    public static NullableTimeOnlyNotHaveValueValidator New(PrincipalChain<TimeOnly?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableTimeOnly.NotHaveValue";

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
