using System.Linq.Expressions;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that a custom expression evaluates to true for the value.
/// </summary>
internal class ReferenceEvaluateExpressionValidator<TSubject>(
    PrincipalChain<TSubject> chain,
    Expression<Func<TSubject, bool>> expression
) : IValidator
{
    public static ReferenceEvaluateExpressionValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        Expression<Func<TSubject, bool>> expression
    ) => new(chain, expression);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Reference.EvaluateExpression";

    public bool Validate()
    {
        var action = expression.Compile();

        if (action(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was not expected to be evaluated to false.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
