using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the memory delta is greater than the specified number of bytes.
/// </summary>
internal class ActionStatsConsumeMemoryGreaterThanValidator(
    PrincipalChain<Model.ActionStats?> chain,
    long bytes
) : IValidator, IRuleDescriptor
{
    public static ActionStatsConsumeMemoryGreaterThanValidator New(
        PrincipalChain<Model.ActionStats?> chain,
        long bytes
    ) => new(chain, bytes);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ActionStats.ConsumeMemoryGreaterThan";
    string IRuleDescriptor.OperationName => "ConsumeMemoryGreaterThan";
    Type IRuleDescriptor.SubjectType => typeof(Model.ActionStats);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = bytes };

    public bool Validate()
    {
        if (chain.GetValue()!.MemoryDelta > bytes)
        {
            return true;
        }

        ResultValidation =
            "Expected memory consumption to be greater than {0} bytes, but it was {1} bytes.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
