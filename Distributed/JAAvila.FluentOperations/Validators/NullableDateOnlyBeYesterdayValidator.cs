using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable dateonly value represents yesterday's date.
/// </summary>
internal class NullableDateOnlyBeYesterdayValidator(PrincipalChain<DateOnly?> chain) : IValidator, IRuleDescriptor
{
    public static NullableDateOnlyBeYesterdayValidator New(PrincipalChain<DateOnly?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateOnly.BeYesterday";
    string IRuleDescriptor.OperationName => "BeYesterday";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value == DateOnly.FromDateTime(DateTime.Today.AddDays(-1)))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be yesterday, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
