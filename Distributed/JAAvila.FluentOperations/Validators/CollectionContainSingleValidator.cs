using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains exactly one element.
/// </summary>
internal class CollectionContainSingleValidator<T>(PrincipalChain<IEnumerable<T>> chain)
    : IValidator
{
    public static CollectionContainSingleValidator<T> New(PrincipalChain<IEnumerable<T>> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.ContainSingle";

    public bool Validate()
    {
        var count = chain.GetValue().Count();

        if (count == 1)
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to contain exactly 1 element, but it had {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
