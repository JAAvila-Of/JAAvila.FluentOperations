using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable DateOnly value equals the expected value.
/// </summary>
internal class NullableDateOnlyBeValidator(PrincipalChain<DateOnly?> chain, DateOnly? expected)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDateOnlyBeValidator New(
        PrincipalChain<DateOnly?> chain,
        DateOnly? expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDateOnly.Be";
    string IRuleDescriptor.OperationName => "Be";
    Type IRuleDescriptor.SubjectType => typeof(DateOnly?);
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
