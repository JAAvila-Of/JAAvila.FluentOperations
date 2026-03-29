using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value is in the future.
/// </summary>
internal class NullableDateTimeOffsetBeInTheFutureValidator(PrincipalChain<DateTimeOffset?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDateTimeOffsetBeInTheFutureValidator New(
        PrincipalChain<DateTimeOffset?> chain
    ) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTimeOffset.BeInTheFuture";
    string IRuleDescriptor.OperationName => "BeInTheFuture";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value > DateTimeOffset.Now)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in the future, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
