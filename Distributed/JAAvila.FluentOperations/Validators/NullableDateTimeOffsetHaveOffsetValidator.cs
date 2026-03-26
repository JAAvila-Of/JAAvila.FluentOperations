using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset value has the expected UTC offset.
/// </summary>
internal class NullableDateTimeOffsetHaveOffsetValidator(
    PrincipalChain<DateTimeOffset?> chain,
    TimeSpan expectedOffset
) : IValidator, IRuleDescriptor
{
    public static NullableDateTimeOffsetHaveOffsetValidator New(
        PrincipalChain<DateTimeOffset?> chain,
        TimeSpan expectedOffset
    ) => new(chain, expectedOffset);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateTimeOffset.HaveOffset";
    string IRuleDescriptor.OperationName => "HaveOffset";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expectedOffset };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Offset == expectedOffset)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have offset {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
