using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>DateTime?</c> values.
/// Supports value presence, null-checking, and inherited nullable operations.
/// </summary>
public class NullableDateTimeOperationsManager
    : BaseNullableOperationsManager<NullableDateTimeOperationsManager, DateTime?>,
        ITestManager<NullableDateTimeOperationsManager, DateTime?>
{
    /// <inheritdoc />
    public PrincipalChain<DateTime?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableDateTimeOperationsManager(DateTime? value, string caller)
    {
        PrincipalChain = PrincipalChain<DateTime?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableDateTimeOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTime.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeHaveValueValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the nullable value is <c>null</c> (does not have a value).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableDateTimeOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTime.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOperationsManager, DateTime?>
            .New(this)
            .WithOperation(NullableDateTimeNotHaveValueValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }
}
