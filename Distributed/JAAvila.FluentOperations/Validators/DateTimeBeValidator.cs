using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value equals the expected value.
/// </summary>
internal class DateTimeBeValidator(PrincipalChain<DateTime> chain, DateTime expected) : IValidator
{
    public static DateTimeBeValidator New(PrincipalChain<DateTime> chain, DateTime expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() == expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
