using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the integer value is within the specified inclusive range.
/// </summary>
internal class IntegerBeInRangeValidator(PrincipalChain<int> chain, int min, int max) : IValidator, IRuleDescriptor
{
    public static IntegerBeInRangeValidator New(PrincipalChain<int> chain, int min, int max) =>
        new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Integer.BeInRange";
    string IRuleDescriptor.OperationName => "BeInRange";
    Type IRuleDescriptor.SubjectType => typeof(int);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min, ["max"] = max };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value >= min && value <= max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
