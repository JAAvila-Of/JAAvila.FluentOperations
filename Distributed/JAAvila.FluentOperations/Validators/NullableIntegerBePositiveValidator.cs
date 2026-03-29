using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is strictly positive.
/// </summary>
internal class NullableIntegerBePositiveValidator(PrincipalChain<int?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableIntegerBePositiveValidator New(PrincipalChain<int?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableInteger.BePositive";
    string IRuleDescriptor.OperationName => "BePositive";
    Type IRuleDescriptor.SubjectType => typeof(int?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value > 0)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be positive, but a non-positive value was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
