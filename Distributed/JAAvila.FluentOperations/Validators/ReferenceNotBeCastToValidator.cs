using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value cannot be cast to the specified type.
/// </summary>
internal class ReferenceNotBeCastToValidator<TSubject>(
    PrincipalChain<TSubject> chain,
    Type expected
) : IValidator, IRuleDescriptor
{
    public static ReferenceNotBeCastToValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        Type expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Reference.NotBeCastTo";
    string IRuleDescriptor.OperationName => "NotBeCastTo";
    Type IRuleDescriptor.SubjectType => typeof(TSubject);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["type"] = expected.FullName ?? expected.Name };

    public bool Validate()
    {
        var type = chain.GetValue()!.GetType();

        if (!type.IsAssignable(expected))
        {
            return true;
        }

        ResultValidation =
            $"The resulting value was not expected to be castable to {expected}, but {type} is.";

        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
