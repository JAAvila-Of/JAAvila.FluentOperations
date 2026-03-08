using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>Guid</c> values.
/// Supports equality, emptiness, format, and version validations.
/// </summary>
public class GuidOperationsManager : ITestManager<GuidOperationsManager, Guid>
{
    /// <inheritdoc />
    public PrincipalChain<Guid> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public GuidOperationsManager(Guid value, string caller)
    {
        PrincipalChain = PrincipalChain<Guid>.Get(value, caller);
        GlobalConfig.Initialize();
    }

    /// <summary>
    /// Asserts that the value is equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <example>
    /// <code>
    /// var id = Guid.NewGuid();
    /// id.Test().Be(id);
    /// </code>
    /// </example>
    public GuidOperationsManager Be(Guid expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Guid.Be))
        {
            return this;
        }

        ExecutionEngine<GuidOperationsManager, Guid>
            .New(this)
            .WithOperation(GuidBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(PrincipalChain.GetValue())
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
    public GuidOperationsManager NotBe(Guid expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Guid.NotBe))
        {
            return this;
        }

        ExecutionEngine<GuidOperationsManager, Guid>
            .New(this)
            .WithOperation(GuidNotBeValidator.New(PrincipalChain, expected))
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

    /// <summary>
    /// Asserts that the value is equal to <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public GuidOperationsManager BeEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Guid.BeEmpty))
        {
            return this;
        }

        ExecutionEngine<GuidOperationsManager, Guid>
            .New(this)
            .WithOperation(GuidBeEmptyValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not equal to <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public GuidOperationsManager NotBeEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Guid.NotBeEmpty))
        {
            return this;
        }

        ExecutionEngine<GuidOperationsManager, Guid>
            .New(this)
            .WithOperation(GuidNotBeEmptyValidator.New(PrincipalChain))
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
    /// Asserts that the value is one of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public GuidOperationsManager BeOneOf(params Guid[] expected)
    {
        return BeOneOf(null, expected);
    }

    /// <summary>
    /// Asserts that the value is one of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expected"/> is empty (empty set guard).
    /// </remarks>
    public GuidOperationsManager BeOneOf(Reason? reason, params Guid[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Guid.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<GuidOperationsManager, Guid>
            .New(this)
            .WithOperation(GuidBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", expected.Select(g => BaseFormatter.Format(g))),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expected.Length == 0,
                        Fail.New(
                            $"The {nameof(BeOneOf)} operation failed because you have not provided any values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not any of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public GuidOperationsManager NotBeOneOf(params Guid[] expected)
    {
        return NotBeOneOf(null, expected);
    }

    /// <summary>
    /// Asserts that the value is not any of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expected"/> is empty (empty set guard).
    /// </remarks>
    public GuidOperationsManager NotBeOneOf(Reason? reason, params Guid[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Guid.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<GuidOperationsManager, Guid>
            .New(this)
            .WithOperation(GuidNotBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", expected.Select(g => BaseFormatter.Format(g)))
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expected.Length == 0,
                        Fail.New(
                            $"The {nameof(NotBeOneOf)} operation failed because you have not provided any values."
                        )
                    )
            )
            .Execute();

        return this;
    }
}
