using JAAvila.FluentOperations.Constraints;
using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string contains the expected element the specified number of times.
/// </summary>
internal class StringContainWithOccurrenceValidator(
    string expected,
    OccurrenceConstraint constraint,
    PrincipalChain<string?> chain
) : IValidator, IRuleDescriptor
{
    public static StringContainWithOccurrenceValidator New(
        string expected,
        OccurrenceConstraint constraint,
        PrincipalChain<string?> chain
    ) => new(expected, constraint, chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.Contain";
    string IRuleDescriptor.OperationName => "Contain";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var coincidences = chain.GetValue().CountCoincidences(expected);

        if (constraint.Validate(coincidences))
        {
            return true;
        }

        ResultValidation =
            $"The value was expected to contain {{0}} {constraint}, but it was found {(coincidences == 1 ? "1 time" : $"{coincidences} times")}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
