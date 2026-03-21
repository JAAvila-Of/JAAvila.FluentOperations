using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the value can be cast to the specified generic type.
/// </summary>
internal class ReferenceBeCastToByGenericValidator<TSubject, TType>(PrincipalChain<TSubject> chain)
    : IValidator
{
    public static ReferenceBeCastToByGenericValidator<TSubject, TType> New(
        PrincipalChain<TSubject> chain
    ) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Reference.BeCastToByGeneric";

    public bool Validate()
    {
        if (chain.GetValue() is TType)
        {
            return true;
        }

        ResultValidation =
            $"The resulting value was supposed to be castable to {typeof(TType)} type.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
