using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value has the expected UTC offset.
/// </summary>
internal class DateTimeOffsetHaveOffsetValidator(
    PrincipalChain<DateTimeOffset> chain,
    TimeSpan expectedOffset
) : IValidator
{
    public static DateTimeOffsetHaveOffsetValidator New(
        PrincipalChain<DateTimeOffset> chain,
        TimeSpan expectedOffset
    ) => new(chain, expectedOffset);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTimeOffset.HaveOffset";

    public bool Validate()
    {
        if (chain.GetValue().Offset == expectedOffset)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have offset {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
