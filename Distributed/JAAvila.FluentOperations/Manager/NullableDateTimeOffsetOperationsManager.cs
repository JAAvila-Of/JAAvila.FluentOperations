using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>DateTimeOffset?</c> values.
/// Supports value presence, null-checking, and inherited nullable operations.
/// </summary>
public class NullableDateTimeOffsetOperationsManager
    : BaseNullableOperationsManager<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>,
        ITestManager<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
{
    /// <inheritdoc />
    public PrincipalChain<DateTimeOffset?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableDateTimeOffsetOperationsManager(DateTimeOffset? value, string caller)
    {
        PrincipalChain = PrincipalChain<DateTimeOffset?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableDateTimeOffsetOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTimeOffset.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetHaveValueValidator.New(PrincipalChain))
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
    public NullableDateTimeOffsetOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableDateTimeOffset.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetNotHaveValueValidator.New(PrincipalChain))
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
    /// Asserts that the value is equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableDateTimeOffsetOperationsManager Be(DateTimeOffset? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.Be))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue()),
                            BaseFormatter.Format(expected)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The value that should not match.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableDateTimeOffsetOperationsManager NotBe(DateTimeOffset? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.DateTimeOffset.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableDateTimeOffsetOperationsManager, DateTimeOffset?>
            .New(this)
            .WithOperation(NullableDateTimeOffsetNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, BaseFormatter.Format(expected))
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }
}
