using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the boolean value and all provided values are false.
/// </summary>
internal class BooleanBeAllFalseValidator(PrincipalChain<bool> chain, bool?[] booleans) : IValidator
{
    public static BooleanBeAllFalseValidator New(PrincipalChain<bool> chain, bool?[] booleans) =>
        new(chain, booleans);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue())
        {
            ResultValidation = "The principal value should have been false.";
            return false;
        }

        if (booleans.Any(x => x == true))
        {
            ResultValidation = "All arguments provided should have been false.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
