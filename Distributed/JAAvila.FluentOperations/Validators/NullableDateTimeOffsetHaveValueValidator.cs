using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable datetimeoffset has a value (is not null).
/// </summary>
internal class NullableDateTimeOffsetHaveValueValidator(PrincipalChain<DateTimeOffset?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDateTimeOffsetHaveValueValidator New(
        PrincipalChain<DateTimeOffset?> chain
    ) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTimeOffset.HaveValue";
    string IRuleDescriptor.OperationName => "HaveValue";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The subject was expected to have a value.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
