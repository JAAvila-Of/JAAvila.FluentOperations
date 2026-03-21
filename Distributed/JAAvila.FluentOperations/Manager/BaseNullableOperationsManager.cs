using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides null-check operations (BeNull, NotBeNull) for managers that support nullable subjects.
/// </summary>
public class BaseNullableOperationsManager<TManager, TSubject>
    where TManager : ITestManager<TManager, TSubject>
{
    private TManager Manager { get; set; }

    /// <summary>
    /// Sets the concrete manager instance used for method chaining returns.
    /// Must be called in the constructor of each derived class.
    /// </summary>
    /// <param name="manager">The concrete manager instance.</param>
    protected void SetManager(TManager manager) => Manager = manager;

    /// <summary>
    /// Asserts that the value is <c>null</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TManager BeNull(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeNull))
        {
            return Manager;
        }

        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(ReferenceBeNullValidator<TSubject>.New(Manager.PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BaseFormatter.Format(Manager.PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return Manager;
    }

    /// <summary>
    /// Asserts that the value is not <c>null</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TManager NotBeNull(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeNull))
        {
            return Manager;
        }

        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(ReferenceNotBeNullValidator<TSubject>.New(Manager.PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return Manager;
    }
}
