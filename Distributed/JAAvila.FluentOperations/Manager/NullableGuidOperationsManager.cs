using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>Guid?</c> values.
/// Supports value presence, null-checking, and inherited nullable operations.
/// </summary>
public class NullableGuidOperationsManager
    : BaseNullableOperationsManager<NullableGuidOperationsManager, Guid?>,
        ITestManager<NullableGuidOperationsManager, Guid?>
{
    /// <inheritdoc />
    public PrincipalChain<Guid?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified nullable value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableGuidOperationsManager(Guid? value, string caller)
    {
        PrincipalChain = PrincipalChain<Guid?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableGuidOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableGuid.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableGuidOperationsManager, Guid?>
            .New(this)
            .WithOperation(NullableGuidHaveValueValidator.New(PrincipalChain))
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
    /// Asserts that the nullable value is <c>null</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableGuidOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableGuid.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableGuidOperationsManager, Guid?>
            .New(this)
            .WithOperation(NullableGuidNotHaveValueValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            PrincipalChain.GetValue()?.ToString() ?? "<null>"
                        )
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
    public NullableGuidOperationsManager Be(Guid? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Guid.Be))
        {
            return this;
        }

        ExecutionEngine<NullableGuidOperationsManager, Guid?>
            .New(this)
            .WithOperation(NullableGuidBeValidator.New(PrincipalChain, expected))
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
    public NullableGuidOperationsManager NotBe(Guid? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Guid.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableGuidOperationsManager, Guid?>
            .New(this)
            .WithOperation(NullableGuidNotBeValidator.New(PrincipalChain, expected))
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
