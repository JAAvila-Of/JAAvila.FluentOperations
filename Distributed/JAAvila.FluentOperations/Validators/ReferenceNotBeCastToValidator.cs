using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value cannot be cast to the specified type.
/// </summary>
internal class ReferenceNotBeCastToValidator<TSubject>(PrincipalChain<TSubject> chain, Type expected)
    : IValidator
{
    public static ReferenceNotBeCastToValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        Type expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
