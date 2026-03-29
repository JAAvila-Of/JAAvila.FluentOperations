using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable long value is outside the specified inclusive range.
/// </summary>
internal class NullableLongNotBeInRangeValidator(PrincipalChain<long?> chain, long min, long max)
    : IValidator,
        IRuleDescriptor
{
    public static NullableLongNotBeInRangeValidator New(
        PrincipalChain<long?> chain,
        long min,
        long max
    ) => new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableLong.NotBeInRange";
    string IRuleDescriptor.OperationName => "NotBeInRange";
    Type IRuleDescriptor.SubjectType => typeof(long?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min, ["max"] = max };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < min || value > max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected not to be in the given range, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
