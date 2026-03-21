using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates the logical implication between the boolean value and the consequent.
/// </summary>
internal class BooleanImplyValidator(PrincipalChain<bool> chain, bool consequent) : IValidator
{
    public static BooleanImplyValidator New(PrincipalChain<bool> chain, bool consequent) =>
        new(chain, consequent);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Boolean.Imply";

    public bool Validate()
    {
        // Logical implication: A => B  ≡  ¬A ∨ B
        if (!chain.GetValue() || consequent)
        {
            return true;
        }

        ResultValidation = "The implication failed: the antecedent was true but the consequent was false.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
