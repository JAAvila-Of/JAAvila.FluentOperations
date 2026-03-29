using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates if the value in the associated <see cref="PrincipalChain{T}"/> of type <c>string?</c> is null.
/// This class implements the <see cref="IValidator"/> interface to enable validation functionality.
/// </summary>
internal class StringBeNullValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    /// <summary>
    /// Creates a new instance of <see cref="StringBeNullValidator"/> with the given <see cref="PrincipalChain{T}"/>.
    /// </summary>
    /// <param name="chain">The <see cref="PrincipalChain{T}"/> to associate with the validator.</param>
    /// <returns>A new initialized instance of <see cref="StringBeNullValidator"/>.</returns>
    public static StringBeNullValidator New(PrincipalChain<string?> chain) => new(chain);

    /// <inheritdoc />
    public string Expected => "Be Null - <null>";

    /// <inheritdoc />
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.BeNull";
    string IRuleDescriptor.OperationName => "BeNull";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    /// <inheritdoc />
    public bool Validate()
    {
        if (chain.GetValue() is null)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be null, but {0} was found.";
        return false;
    }

    /// <inheritdoc />
    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
