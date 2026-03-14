using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the memory delta is less than the specified number of bytes.
/// </summary>
internal class ActionStatsConsumeMemoryLessThanValidator(PrincipalChain<Model.ActionStats?> chain, long bytes) : IValidator
{
    public static ActionStatsConsumeMemoryLessThanValidator New(PrincipalChain<Model.ActionStats?> chain, long bytes) =>
        new(chain, bytes);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue()!.MemoryDelta < bytes)
        {
            return true;
        }

        ResultValidation = "Expected memory consumption to be less than {0} bytes, but it was {1} bytes.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
