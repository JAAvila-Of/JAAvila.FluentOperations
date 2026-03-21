using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable boolean value and all provided values are false.
/// </summary>
internal class NullableBooleanBeAllFalseValidator(PrincipalChain<bool?> chain, bool?[] booleans)
    : IValidator, IRuleDescriptor
{
    public static NullableBooleanBeAllFalseValidator New(
        PrincipalChain<bool?> chain,
        bool?[] booleans
    ) => new(chain, booleans);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableBoolean.BeAllFalse";
    string IRuleDescriptor.OperationName => "BeAllFalse";
    Type IRuleDescriptor.SubjectType => typeof(bool?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = booleans };

    public bool Validate()
    {
        if (chain.GetValue() == true)
        {
            ResultValidation = "The principal value should have been false.";
            return false;
        }

        // ReSharper disable once InvertIf
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
