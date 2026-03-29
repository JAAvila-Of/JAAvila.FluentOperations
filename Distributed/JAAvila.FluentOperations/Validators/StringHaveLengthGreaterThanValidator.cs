using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string length is strictly greater than the expected value.
/// </summary>
internal class StringHaveLengthGreaterThanValidator(PrincipalChain<string?> chain, int expected)
    : IValidator,
        IRuleDescriptor
{
    public static StringHaveLengthGreaterThanValidator New(
        PrincipalChain<string?> chain,
        int expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.HaveLengthGreaterThan";
    string IRuleDescriptor.OperationName => "HaveLengthGreaterThan";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Length > expected)
        {
            return true;
        }

        ResultValidation =
            "The value was expected to have a length greater than {0}, but the actual length was {1}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
