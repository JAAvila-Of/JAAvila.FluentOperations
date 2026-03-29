using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value can be cast to the expected type.
/// </summary>
internal class ReferenceBeCastToValidator<TSubject>(PrincipalChain<TSubject> chain, Type expected)
    : IValidator,
        IRuleDescriptor
{
    public static ReferenceBeCastToValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        Type expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Reference.BeCastTo";
    string IRuleDescriptor.OperationName => "BeCastTo";
    Type IRuleDescriptor.SubjectType => typeof(TSubject);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["type"] = expected.FullName ?? expected.Name };

    public bool Validate()
    {
        var type = chain.GetValue()!.GetType();

        if (type.IsAssignable(expected))
        {
            return true;
        }

        ResultValidation = $"The resulting value was supposed to be castable to {expected} type.";

        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
