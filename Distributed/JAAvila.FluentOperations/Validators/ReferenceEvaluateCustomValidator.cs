using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Wraps an <see cref="ICustomValidator{T}"/> into the internal <see cref="IValidator"/> contract.
/// </summary>
internal class ReferenceEvaluateCustomValidator<TSubject>(
    PrincipalChain<TSubject> chain,
    ICustomValidator<TSubject> customValidator
) : IValidator
{
    public static ReferenceEvaluateCustomValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        ICustomValidator<TSubject> customValidator
    ) => new(chain, customValidator);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (customValidator.IsValid(value))
        {
            return true;
        }

        ResultValidation = customValidator.GetFailureMessage(value);
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
