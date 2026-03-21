using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable long value is strictly positive.
/// </summary>
internal class NullableLongBePositiveValidator(PrincipalChain<long?> chain) : IValidator, IRuleDescriptor
{
    public static NullableLongBePositiveValidator New(PrincipalChain<long?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableLong.BePositive";
    string IRuleDescriptor.OperationName => "BePositive";
    Type IRuleDescriptor.SubjectType => typeof(long?);
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
