using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value can be cast to the expected type.
/// </summary>
internal class ReferenceBeCastToValidator<TSubject>(PrincipalChain<TSubject> chain, Type expected)
    : IValidator
{
    public static ReferenceBeCastToValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        Type expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
