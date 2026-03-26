using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value has the expected month.
/// </summary>
internal class NullableDateTimeHaveMonthValidator(PrincipalChain<DateTime?> chain, int expectedMonth) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeHaveMonthValidator New(PrincipalChain<DateTime?> chain, int expectedMonth) =>
        new(chain, expectedMonth);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTime.HaveMonth";
    string IRuleDescriptor.OperationName => "HaveMonth";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedMonth };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Month == expectedMonth)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have month {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
