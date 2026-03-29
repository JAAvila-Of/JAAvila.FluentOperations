using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable dateonly value has the expected month.
/// </summary>
internal class NullableDateOnlyHaveMonthValidator(
    PrincipalChain<DateOnly?> chain,
    int expectedMonth
) : IValidator, IRuleDescriptor
{
    public static NullableDateOnlyHaveMonthValidator New(
        PrincipalChain<DateOnly?> chain,
        int expectedMonth
    ) => new(chain, expectedMonth);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateOnly.HaveMonth";
    string IRuleDescriptor.OperationName => "HaveMonth";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly?);
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
