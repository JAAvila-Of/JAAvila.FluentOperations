using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Wraps an <see cref="IAsyncCustomValidator{T}"/> into the internal <see cref="IValidator"/> contract.
/// </summary>
internal class ReferenceEvaluateAsyncCustomValidator<TSubject>(
    PrincipalChain<TSubject> chain,
    IAsyncCustomValidator<TSubject> customValidator
) : IValidator
{
    public static ReferenceEvaluateAsyncCustomValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        IAsyncCustomValidator<TSubject> customValidator
    ) => new(chain, customValidator);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        // Synchronous fallback -- block on the async validator
        return ValidateAsync().GetAwaiter().GetResult();
    }

    public async Task<bool> ValidateAsync()
    {
        var value = chain.GetValue();

        if (await customValidator.IsValidAsync(value))
        {
            return true;
        }

        ResultValidation = customValidator.GetFailureMessage(value);
        return false;
    }
}
