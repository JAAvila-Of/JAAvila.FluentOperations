using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timeonly value is before the expected value.
/// </summary>
internal class NullableTimeOnlyBeBeforeValidator(PrincipalChain<TimeOnly?> chain, TimeOnly expected)
    : IValidator,
        IRuleDescriptor
{
    public static NullableTimeOnlyBeBeforeValidator New(
        PrincipalChain<TimeOnly?> chain,
        TimeOnly expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableTimeOnly.BeBefore";
    string IRuleDescriptor.OperationName => "BeBefore";
    Type IRuleDescriptor.SubjectType => typeof(TimeOnly?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Value < expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be before {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
