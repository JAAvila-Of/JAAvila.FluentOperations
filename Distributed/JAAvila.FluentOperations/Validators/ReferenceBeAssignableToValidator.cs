using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value's runtime type is assignable to the expected type.
/// Uses Type.IsAssignableFrom (checks inheritance chain + interface implementation).
/// </summary>
internal class ReferenceBeAssignableToValidator(
    PrincipalChain<object?> chain,
    Type expected
) : IValidator, IRuleDescriptor
{
    public static ReferenceBeAssignableToValidator New(
        PrincipalChain<object?> chain,
        Type expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Reference.BeAssignableTo";
    string IRuleDescriptor.OperationName => "BeAssignableTo";
    Type IRuleDescriptor.SubjectType => typeof(object);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["type"] = expected.FullName ?? expected.Name };

    public bool Validate()
    {
        var value = chain.GetValue();
        var actualType = value!.GetType();

        if (expected.IsAssignableFrom(actualType))
        {
            return true;
        }

        ResultValidation =
            $"Expected type {actualType.FullName} to be assignable to {expected.FullName}, but it is not.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
