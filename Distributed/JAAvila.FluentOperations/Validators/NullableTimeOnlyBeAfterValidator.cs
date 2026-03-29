using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timeonly value is after the expected value.
/// </summary>
internal class NullableTimeOnlyBeAfterValidator(PrincipalChain<TimeOnly?> chain, TimeOnly expected)
    : IValidator,
        IRuleDescriptor
{
    public static NullableTimeOnlyBeAfterValidator New(
        PrincipalChain<TimeOnly?> chain,
        TimeOnly expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableTimeOnly.BeAfter";
    string IRuleDescriptor.OperationName => "BeAfter";
    Type IRuleDescriptor.SubjectType => typeof(TimeOnly?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Value > expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be after {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
