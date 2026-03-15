using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection has more elements than the expected value.
/// </summary>
internal class CollectionHaveLengthGreaterThanValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    int expected
) : IValidator
{
    public static CollectionHaveLengthGreaterThanValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var count = chain.GetValue().Count();

        if (count > expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to have length greater than {0}, but it had {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
