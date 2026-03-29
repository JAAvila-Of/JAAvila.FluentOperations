using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the array length is greater than the expected value.
/// </summary>
internal class ArrayHaveLengthGreaterThanValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    int expected
) : IValidator, IRuleDescriptor
{
    public static ArrayHaveLengthGreaterThanValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    private T[] A => chain.GetValue().ToArray();
    public string MessageKey => "Array.HaveLengthGreaterThan";
    string IRuleDescriptor.OperationName => "HaveLengthGreaterThan";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = A, ["value"] = expected };

    public bool Validate()
    {
        if (A.Length > expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting array was expected to have length greater than {0}, but it had {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
