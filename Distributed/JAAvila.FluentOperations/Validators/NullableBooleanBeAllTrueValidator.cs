using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable boolean value and all provided values are true.
/// </summary>
internal class NullableBooleanBeAllTrueValidator(PrincipalChain<bool?> chain, bool?[] booleans)
    : IValidator
{
    public static NullableBooleanBeAllTrueValidator New(
        PrincipalChain<bool?> chain,
        bool?[] booleans
    ) => new(chain, booleans);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableBoolean.BeAllTrue";

    public bool Validate()
    {
        if (chain.GetValue() == false)
        {
            ResultValidation = "The principal value should have been true.";
            return false;
        }

        // ReSharper disable once InvertIf
        if (booleans.Any(x => x == false))
        {
            ResultValidation = "All arguments provided should have been true.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
