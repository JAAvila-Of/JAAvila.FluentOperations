using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the array length is less than the expected value.
/// </summary>
internal class ArrayHaveLengthLessThanValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    int expected
) : IValidator, IRuleDescriptor
{
    public static ArrayHaveLengthLessThanValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    private T[] A => chain.GetValue().ToArray();
    public string MessageKey => "Array.HaveLengthLessThan";
    string IRuleDescriptor.OperationName => "HaveLengthLessThan";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = A, ["value"] = expected };

    public bool Validate()
    {
        if (A.Length < expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting array was expected to have length less than {0}, but it had {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
