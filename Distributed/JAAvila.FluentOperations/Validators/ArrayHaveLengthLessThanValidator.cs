using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the array length is less than the expected value.
/// </summary>
internal class ArrayHaveLengthLessThanValidator<T>(PrincipalChain<IEnumerable<T>> chain, T[] array, int expected) : IValidator
{
    public static ArrayHaveLengthLessThanValidator<T> New(PrincipalChain<IEnumerable<T>> chain, T[] array, int expected) =>
        new(chain, array, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Array.HaveLengthLessThan";

    public bool Validate()
    {
        if (array.Length < expected)
        {
            return true;
        }

        ResultValidation = "The resulting array was expected to have length less than {0}, but it had {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
