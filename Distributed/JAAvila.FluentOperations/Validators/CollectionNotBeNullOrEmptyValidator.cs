using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection is not null or empty.
/// </summary>
internal class CollectionNotBeNullOrEmptyValidator<T>(PrincipalChain<IEnumerable<T>> chain)
    : IValidator
{
    public static CollectionNotBeNullOrEmptyValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain
    ) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.NotBeNullOrEmpty";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!value.IsNullOrEmpty())
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to be non-null and non-empty, but it was {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
