using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ulong value is outside the specified inclusive range.
/// </summary>
internal class ULongNotBeInRangeValidator(PrincipalChain<ulong> chain, ulong min, ulong max)
    : IValidator,
        IRuleDescriptor
{
    public static ULongNotBeInRangeValidator New(
        PrincipalChain<ulong> chain,
        ulong min,
        ulong max
    ) => new(chain, min, max);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ULong.NotBeInRange";
    string IRuleDescriptor.OperationName => "NotBeInRange";
    Type IRuleDescriptor.SubjectType => typeof(ulong);
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
            "The resulting value was expected to not be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
