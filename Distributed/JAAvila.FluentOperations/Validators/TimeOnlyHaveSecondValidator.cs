using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the timeonly value has the expected second.
/// </summary>
internal class TimeOnlyHaveSecondValidator(PrincipalChain<TimeOnly> chain, int second) : IValidator
{
    public static TimeOnlyHaveSecondValidator New(PrincipalChain<TimeOnly> chain, int second) =>
        new(chain, second);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue().Second == second)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have second {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
