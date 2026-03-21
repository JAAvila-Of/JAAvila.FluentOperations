using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value's runtime type is NOT assignable to the expected type.
/// </summary>
internal class ReferenceNotBeAssignableToValidator(
    PrincipalChain<object?> chain,
    Type expected
) : IValidator, IRuleDescriptor
{
    public static ReferenceNotBeAssignableToValidator New(
        PrincipalChain<object?> chain,
        Type expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Reference.NotBeAssignableTo";
    string IRuleDescriptor.OperationName => "NotBeAssignableTo";
    Type IRuleDescriptor.SubjectType => typeof(object);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["type"] = expected.FullName ?? expected.Name };

    public bool Validate()
    {
        var value = chain.GetValue();
        var actualType = value!.GetType();

        if (!expected.IsAssignableFrom(actualType))
        {
            return true;
        }

        ResultValidation =
            $"Expected type {actualType.FullName} to not be assignable to {expected.FullName}, but it is.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
