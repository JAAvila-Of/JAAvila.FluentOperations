using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the boolean value and all provided values are true.
/// </summary>
internal class BooleanBeAllTrueValidator(PrincipalChain<bool> chain, bool?[] booleans) : IValidator, IRuleDescriptor
{
    public static BooleanBeAllTrueValidator New(PrincipalChain<bool> chain, bool?[] booleans) =>
        new(chain, booleans);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Boolean.BeAllTrue";
    string IRuleDescriptor.OperationName => "BeAllTrue";
    Type IRuleDescriptor.SubjectType => typeof(bool);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = booleans };

    public bool Validate()
    {
        if (!chain.GetValue())
        {
            ResultValidation = "The principal value should have been true.";
            return false;
        }

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
