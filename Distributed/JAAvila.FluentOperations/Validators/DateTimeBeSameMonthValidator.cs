using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value falls in the same month as the expected value.
/// </summary>
internal class DateTimeBeSameMonthValidator(PrincipalChain<DateTime> chain, DateTime expected)
    : IValidator,
        IRuleDescriptor
{
    public static DateTimeBeSameMonthValidator New(
        PrincipalChain<DateTime> chain,
        DateTime expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "DateTime.BeSameMonth";
    string IRuleDescriptor.OperationName => "BeSameMonth";
    Type IRuleDescriptor.SubjectType => typeof(DateTime);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var v = chain.GetValue();

        if (v.Year == expected.Year && v.Month == expected.Month)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the same month as {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
