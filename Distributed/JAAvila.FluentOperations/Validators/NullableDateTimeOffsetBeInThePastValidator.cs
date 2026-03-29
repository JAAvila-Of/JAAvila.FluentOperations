using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value is in the past.
/// </summary>
internal class NullableDateTimeOffsetBeInThePastValidator(PrincipalChain<DateTimeOffset?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDateTimeOffsetBeInThePastValidator New(
        PrincipalChain<DateTimeOffset?> chain
    ) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTimeOffset.BeInThePast";
    string IRuleDescriptor.OperationName => "BeInThePast";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value < DateTimeOffset.Now)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be in the past, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
