using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string length is strictly less than the expected value.
/// </summary>
internal class StringHaveLengthLessThanValidator(PrincipalChain<string?> chain, int expected)
    : IValidator, IRuleDescriptor
{
    public static StringHaveLengthLessThanValidator New(PrincipalChain<string?> chain, int expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.HaveLengthLessThan";
    string IRuleDescriptor.OperationName => "HaveLengthLessThan";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        if (chain.GetValue()!.Length < expected)
        {
            return true;
        }

        ResultValidation =
            "The value was expected to have a length less than {0}, but the actual length was {1}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
