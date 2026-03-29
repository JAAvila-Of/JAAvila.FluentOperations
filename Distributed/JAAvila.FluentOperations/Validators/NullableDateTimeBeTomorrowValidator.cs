using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value represents tomorrow's date.
/// </summary>
internal class NullableDateTimeBeTomorrowValidator(PrincipalChain<DateTime?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDateTimeBeTomorrowValidator New(PrincipalChain<DateTime?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTime.BeTomorrow";
    string IRuleDescriptor.OperationName => "BeTomorrow";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Date == DateTime.Today.AddDays(1))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be tomorrow, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
