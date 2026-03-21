using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value does not equal the expected value.
/// </summary>
internal class DateTimeNotBeValidator(PrincipalChain<DateTime> chain, DateTime expected) : IValidator
{
    public static DateTimeNotBeValidator New(PrincipalChain<DateTime> chain, DateTime expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTime.NotBe";

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
