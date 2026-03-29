using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable ulong value is greater than the expected value.
/// </summary>
internal class NullableULongBeGreaterThanValidator(PrincipalChain<ulong?> chain, ulong comparison)
    : IValidator,
        IRuleDescriptor
{
    public static NullableULongBeGreaterThanValidator New(
        PrincipalChain<ulong?> chain,
        ulong comparison
    ) => new(chain, comparison);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableULong.BeGreaterThan";
    string IRuleDescriptor.OperationName => "BeGreaterThan";
    Type IRuleDescriptor.SubjectType => typeof(ulong?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = comparison };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value > comparison)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be greater than the given value, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
