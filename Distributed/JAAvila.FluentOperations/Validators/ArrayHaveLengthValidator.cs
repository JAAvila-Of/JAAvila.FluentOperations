using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the array has the expected length.
/// </summary>
internal class ArrayHaveLengthValidator<T>(PrincipalChain<IEnumerable<T>> chain, int expected)
    : IValidator,
        IRuleDescriptor
{
    public static ArrayHaveLengthValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    private T[] A => chain.GetValue().ToArray();
    public string MessageKey => "Array.HaveLength";
    string IRuleDescriptor.OperationName => "HaveLength";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = A, ["value"] = expected };

    public bool Validate()
    {
        if (A.Length == expected)
        {
            return true;
        }

        ResultValidation = "The resulting array was expected to have length {0}, but it had {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
