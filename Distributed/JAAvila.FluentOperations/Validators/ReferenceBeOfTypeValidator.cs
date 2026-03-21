using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value is of the expected type.
/// </summary>
internal class ReferenceBeOfTypeValidator<TSubject>(PrincipalChain<TSubject> chain, Type expected)
    : IValidator
{
    public static ReferenceBeOfTypeValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        Type expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Reference.BeOfType";

    public bool Validate()
    {
        var value = chain.GetValue()!.GetType();
        var fType = value;

        if (expected.IsGenericTypeDefinition && value.IsGenericType)
        {
            fType = value.GetGenericTypeDefinition();
        }

        if (fType == expected)
        {
            return true;
        }

        var fmType = fType.FullName;
        var fmExpectedType = expected.FullName;

        ResultValidation =
            fmType == fmExpectedType
                ? $"Type [{expected.AssemblyQualifiedName}] {fmExpectedType} was expected but type [{fType.AssemblyQualifiedName}] {fmType} was found"
                : $"Type {fmExpectedType} was expected but type {fmType} was found";

        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
