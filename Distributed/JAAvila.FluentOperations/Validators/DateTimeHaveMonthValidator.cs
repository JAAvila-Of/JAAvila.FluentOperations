using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value has the expected month.
/// </summary>
internal class DateTimeHaveMonthValidator(PrincipalChain<DateTime> chain, int expectedMonth)
    : IValidator,
        IRuleDescriptor
{
    public static DateTimeHaveMonthValidator New(
        PrincipalChain<DateTime> chain,
        int expectedMonth
    ) => new(chain, expectedMonth);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "DateTime.HaveMonth";
    string IRuleDescriptor.OperationName => "HaveMonth";
    Type IRuleDescriptor.SubjectType => typeof(DateTime);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedMonth };

    public bool Validate()
    {
        if (chain.GetValue().Month == expectedMonth)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have month {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
