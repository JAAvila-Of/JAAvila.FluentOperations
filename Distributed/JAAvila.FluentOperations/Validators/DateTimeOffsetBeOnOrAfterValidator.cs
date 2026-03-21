using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value is on or after the expected value.
/// </summary>
internal class DateTimeOffsetBeOnOrAfterValidator(PrincipalChain<DateTimeOffset> chain, DateTimeOffset expected) : IValidator
{
    public static DateTimeOffsetBeOnOrAfterValidator New(PrincipalChain<DateTimeOffset> chain, DateTimeOffset expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTimeOffset.BeOnOrAfter";

    public bool Validate()
    {
        if (chain.GetValue() >= expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be on or after {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
