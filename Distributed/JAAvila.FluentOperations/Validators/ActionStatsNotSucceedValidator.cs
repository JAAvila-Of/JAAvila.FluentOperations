using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the action threw an exception (did not succeed).
/// </summary>
internal class ActionStatsNotSucceedValidator(PrincipalChain<Model.ActionStats?> chain) : IValidator
{
    public static ActionStatsNotSucceedValidator New(PrincipalChain<Model.ActionStats?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!chain.GetValue()!.Succeeded)
        {
            return true;
        }

        ResultValidation = "Expected the action to fail, but it succeeded.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
