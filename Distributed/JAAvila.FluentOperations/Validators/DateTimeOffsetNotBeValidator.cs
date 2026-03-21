using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the datetimeoffset value does not equal the expected value.
/// </summary>
internal class DateTimeOffsetNotBeValidator(PrincipalChain<DateTimeOffset> chain, DateTimeOffset expected) : IValidator, IRuleDescriptor
{
    public static DateTimeOffsetNotBeValidator New(PrincipalChain<DateTimeOffset> chain, DateTimeOffset expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "DateTimeOffset.NotBe";
    string IRuleDescriptor.OperationName => "NotBe";
    Type IRuleDescriptor.SubjectType => typeof(DateTimeOffset);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be {0}, but it was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
