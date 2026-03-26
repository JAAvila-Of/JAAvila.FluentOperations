using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable dateonly value is strictly after the expected value.
/// </summary>
internal class NullableDateOnlyBeAfterValidator(PrincipalChain<DateOnly?> chain, DateOnly expected) : IValidator, IRuleDescriptor
{
    public static NullableDateOnlyBeAfterValidator New(PrincipalChain<DateOnly?> chain, DateOnly expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateOnly.BeAfter";
    string IRuleDescriptor.OperationName => "BeAfter";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly?);
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
