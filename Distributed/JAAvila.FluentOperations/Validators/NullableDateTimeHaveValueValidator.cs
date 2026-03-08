using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime has a value (is not null).
/// </summary>
internal class NullableDateTimeHaveValueValidator(PrincipalChain<DateTime?> chain) : IValidator
{
    public static NullableDateTimeHaveValueValidator New(PrincipalChain<DateTime?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
