using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timeonly value does not equal the expected value.
/// </summary>
internal class TimeOnlyNotBeValidator(PrincipalChain<TimeOnly> chain, TimeOnly expected) : IValidator
{
    public static TimeOnlyNotBeValidator New(PrincipalChain<TimeOnly> chain, TimeOnly expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "TimeOnly.NotBe";

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
