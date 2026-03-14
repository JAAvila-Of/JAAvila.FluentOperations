using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the action took longer than the specified minimum duration.
/// </summary>
internal class ActionStatsTakeLongerThanValidator(PrincipalChain<Model.ActionStats?> chain, TimeSpan minDuration) : IValidator
{
    public static ActionStatsTakeLongerThanValidator New(PrincipalChain<Model.ActionStats?> chain, TimeSpan minDuration) =>
        new(chain, minDuration);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue()!.ElapsedTime >= minDuration)
        {
            return true;
        }

        ResultValidation = "Expected the action to take longer than {0}, but it completed in {1}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
