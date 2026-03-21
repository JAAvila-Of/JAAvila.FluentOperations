using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is strictly negative.
/// </summary>
internal class NullableIntegerBeNegativeValidator(PrincipalChain<int?> chain) : IValidator, IRuleDescriptor
{
    public static NullableIntegerBeNegativeValidator New(PrincipalChain<int?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableInteger.BeNegative";
    string IRuleDescriptor.OperationName => "BeNegative";
    Type IRuleDescriptor.SubjectType => typeof(int?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < 0)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be negative, but a non-negative value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
