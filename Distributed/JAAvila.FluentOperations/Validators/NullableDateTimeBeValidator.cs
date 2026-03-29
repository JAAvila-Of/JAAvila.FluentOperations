using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable DateTime value equals the expected value.
/// </summary>
internal class NullableDateTimeBeValidator(PrincipalChain<DateTime?> chain, DateTime? expected)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDateTimeBeValidator New(
        PrincipalChain<DateTime?> chain,
        DateTime? expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateTime.Be";
    string IRuleDescriptor.OperationName => "Be";
    Type IRuleDescriptor.SubjectType => typeof(DateTime?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected! };

    public bool Validate()
    {
        if (chain.GetValue() == expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
