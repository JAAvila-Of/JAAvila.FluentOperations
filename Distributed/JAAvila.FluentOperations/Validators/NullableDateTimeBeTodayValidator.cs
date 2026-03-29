using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value represents today's date.
/// </summary>
internal class NullableDateTimeBeTodayValidator(PrincipalChain<DateTime?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDateTimeBeTodayValidator New(PrincipalChain<DateTime?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTime.BeToday";
    string IRuleDescriptor.OperationName => "BeToday";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Date == DateTime.Today)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be today, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
