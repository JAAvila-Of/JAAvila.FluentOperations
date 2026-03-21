using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the date value represents tomorrow.
/// </summary>
internal class DateOnlyBeTomorrowValidator(PrincipalChain<DateOnly> chain) : IValidator, IRuleDescriptor
{
    public static DateOnlyBeTomorrowValidator New(PrincipalChain<DateOnly> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateOnly.BeTomorrow";
    string IRuleDescriptor.OperationName => "BeTomorrow";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() == DateOnly.FromDateTime(DateTime.Today.AddDays(1)))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be tomorrow, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
