using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value falls in the same calendar month and year as the expected value.
/// </summary>
internal class NullableDateTimeBeSameMonthValidator(
    PrincipalChain<DateTime?> chain,
    DateTime expected
) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeBeSameMonthValidator New(
        PrincipalChain<DateTime?> chain,
        DateTime expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTime.BeSameMonth";
    string IRuleDescriptor.OperationName => "BeSameMonth";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var v = chain.GetValue()!.Value;

        if (v.Year == expected.Year && v.Month == expected.Month)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the same month as {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
