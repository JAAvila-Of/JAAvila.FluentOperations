using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value is after the expected value.
/// </summary>
internal class DateTimeBeAfterValidator(PrincipalChain<DateTime> chain, DateTime expected) : IValidator
{
    public static DateTimeBeAfterValidator New(PrincipalChain<DateTime> chain, DateTime expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTime.BeAfter";

    public bool Validate()
    {
        if (chain.GetValue() > expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be after {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
