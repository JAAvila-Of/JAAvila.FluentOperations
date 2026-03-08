using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value is not of the expected type.
/// </summary>
internal class ReferenceNotBeOfTypeValidator<TSubject>(
    PrincipalChain<TSubject> chain,
    Type expected
) : IValidator
{
    public static ReferenceNotBeOfTypeValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        Type expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue()!.GetType();
        var fType = value;

        if (expected.IsGenericTypeDefinition && value.IsGenericType)
        {
            fType = value.GetGenericTypeDefinition();
        }

        if (fType != expected)
        {
            return true;
        }

        var fmExpectedType = expected.FullName;

        ResultValidation = $"The type found was expected to not be equal to {fmExpectedType}";

        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
