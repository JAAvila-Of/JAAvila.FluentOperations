using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetime value is in the future.
/// </summary>
internal class NullableDateTimeBeInTheFutureValidator(PrincipalChain<DateTime?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDateTimeBeInTheFutureValidator New(PrincipalChain<DateTime?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTime.BeInTheFuture";
    string IRuleDescriptor.OperationName => "BeInTheFuture";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value > DateTime.Now)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the future, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
