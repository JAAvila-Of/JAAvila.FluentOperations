using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value represents tomorrow.
/// </summary>
internal class DateTimeBeTomorrowValidator(PrincipalChain<DateTime> chain)
    : IValidator,
        IRuleDescriptor
{
    public static DateTimeBeTomorrowValidator New(PrincipalChain<DateTime> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "DateTime.BeTomorrow";
    string IRuleDescriptor.OperationName => "BeTomorrow";
    Type IRuleDescriptor.SubjectType => typeof(DateTime);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue().Date == DateTime.Today.AddDays(1))
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
