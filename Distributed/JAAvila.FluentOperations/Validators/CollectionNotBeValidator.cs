using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection reference does not equal the expected reference (reference equality).
/// </summary>
internal class CollectionNotBeValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    IEnumerable<T> expected
) : IValidator
{
    public static CollectionNotBeValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        IEnumerable<T> expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!ReferenceEquals(value, expected))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to not be the same reference as {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
