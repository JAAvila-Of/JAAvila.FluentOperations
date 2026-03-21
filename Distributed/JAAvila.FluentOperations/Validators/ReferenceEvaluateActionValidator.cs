using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that a custom action evaluates successfully against the value.
/// </summary>
internal class ReferenceEvaluateActionValidator<TSubject, TType>(
    PrincipalChain<TSubject> chain,
    Action<TType> action
) : IValidator
    where TType : TSubject
{
    public static ReferenceEvaluateActionValidator<TSubject, TType> New(
        PrincipalChain<TSubject> chain,
        Action<TType> action
    ) => new(chain, action);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Reference.EvaluateAction";

    public bool Validate()
    {
        action((TType)chain.GetValue()!);

        return true;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
