using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection is not empty.
/// </summary>
internal class CollectionNotBeEmptyValidator<T>(PrincipalChain<IEnumerable<T>> chain) : IValidator
{
    public static CollectionNotBeEmptyValidator<T> New(PrincipalChain<IEnumerable<T>> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.NotBeEmpty";

    public bool Validate()
    {
        if (chain.GetValue().Any())
        {
            return true;
        }

        ResultValidation = "The resulting collection was expected to be non-empty, but it was empty.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
