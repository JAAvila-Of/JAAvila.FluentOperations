using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable dateonly value represents tomorrow's date.
/// </summary>
internal class NullableDateOnlyBeTomorrowValidator(PrincipalChain<DateOnly?> chain) : IValidator, IRuleDescriptor
{
    public static NullableDateOnlyBeTomorrowValidator New(PrincipalChain<DateOnly?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateOnly.BeTomorrow";
    string IRuleDescriptor.OperationName => "BeTomorrow";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value == DateOnly.FromDateTime(DateTime.Today.AddDays(1)))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be tomorrow, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
