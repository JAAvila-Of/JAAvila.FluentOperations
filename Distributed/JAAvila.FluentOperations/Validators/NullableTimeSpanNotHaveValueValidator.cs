using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timespan does not have a value (is null).
/// </summary>
internal class NullableTimeSpanNotHaveValueValidator(PrincipalChain<TimeSpan?> chain) : IValidator
{
    public static NullableTimeSpanNotHaveValueValidator New(PrincipalChain<TimeSpan?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableTimeSpan.NotHaveValue";

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
