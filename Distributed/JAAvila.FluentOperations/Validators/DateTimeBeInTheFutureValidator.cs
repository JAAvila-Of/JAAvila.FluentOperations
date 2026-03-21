using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetime value is in the future.
/// </summary>
internal class DateTimeBeInTheFutureValidator(PrincipalChain<DateTime> chain) : IValidator, IRuleDescriptor
{
    public static DateTimeBeInTheFutureValidator New(PrincipalChain<DateTime> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTime.BeInTheFuture";
    string IRuleDescriptor.OperationName => "BeInTheFuture";
    Type IRuleDescriptor.SubjectType => typeof(DateTime);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() > DateTime.Now)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be in the future, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
