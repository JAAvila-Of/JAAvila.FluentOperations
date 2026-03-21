using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection is empty.
/// </summary>
internal class CollectionBeEmptyValidator<T>(PrincipalChain<IEnumerable<T>> chain) : IValidator
{
    public static CollectionBeEmptyValidator<T> New(PrincipalChain<IEnumerable<T>> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.BeEmpty";

    public bool Validate()
    {
        if (!chain.GetValue().Any())
        {
            return true;
        }

        ResultValidation = "The resulting collection was expected to be empty, but it contained {0} element(s).";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
