using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value is in the future.
/// </summary>
internal class DateTimeOffsetBeInTheFutureValidator(PrincipalChain<DateTimeOffset> chain)
    : IValidator
{
    public static DateTimeOffsetBeInTheFutureValidator New(PrincipalChain<DateTimeOffset> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTimeOffset.BeInTheFuture";

    public bool Validate()
    {
        if (chain.GetValue() > DateTimeOffset.UtcNow)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the future, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
