using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value falls on the same day as the expected value.
/// </summary>
internal class DateTimeOffsetBeSameDayValidator(
    PrincipalChain<DateTimeOffset> chain,
    DateTimeOffset expected
) : IValidator, IRuleDescriptor
{
    public static DateTimeOffsetBeSameDayValidator New(
        PrincipalChain<DateTimeOffset> chain,
        DateTimeOffset expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTimeOffset.BeSameDay";
    string IRuleDescriptor.OperationName => "BeSameDay";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (
            value.Year == expected.Year
            && value.Month == expected.Month
            && value.Day == expected.Day
        )
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be the same day as {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
