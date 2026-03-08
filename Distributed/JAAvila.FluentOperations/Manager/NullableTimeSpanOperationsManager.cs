using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>TimeSpan?</c> values.
/// Supports value presence, null-checking, and inherited nullable operations.
/// </summary>
public class NullableTimeSpanOperationsManager
    : BaseNullableOperationsManager<NullableTimeSpanOperationsManager, TimeSpan?>,
        ITestManager<NullableTimeSpanOperationsManager, TimeSpan?>
{
    /// <inheritdoc />
    public PrincipalChain<TimeSpan?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableTimeSpanOperationsManager(TimeSpan? value, string caller)
    {
        PrincipalChain = PrincipalChain<TimeSpan?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableTimeSpanOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableTimeSpan.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanHaveValueValidator.New(PrincipalChain))
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
    public NullableTimeSpanOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableTimeSpan.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableTimeSpanOperationsManager, TimeSpan?>
            .New(this)
            .WithOperation(NullableTimeSpanNotHaveValueValidator.New(PrincipalChain))
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
