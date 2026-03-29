using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the memory delta is less than the specified number of bytes.
/// </summary>
internal class ActionStatsConsumeMemoryLessThanValidator(
    PrincipalChain<Model.ActionStats?> chain,
    long bytes
) : IValidator, IRuleDescriptor
{
    public static ActionStatsConsumeMemoryLessThanValidator New(
        PrincipalChain<Model.ActionStats?> chain,
        long bytes
    ) => new(chain, bytes);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ActionStats.ConsumeMemoryLessThan";
    string IRuleDescriptor.OperationName => "ConsumeMemoryLessThan";
    Type IRuleDescriptor.SubjectType => typeof(Model.ActionStats);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = bytes };

    public bool Validate()
    {
        if (chain.GetValue()!.MemoryDelta < bytes)
        {
            return true;
        }

        ResultValidation =
            "Expected memory consumption to be less than {0} bytes, but it was {1} bytes.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
