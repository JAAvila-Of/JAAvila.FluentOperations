using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>bool</c> values.
/// Supports equality, collective truth, and logical implication validations.
/// </summary>
public class BooleanOperationsManager : ITestManager<BooleanOperationsManager, bool>
{
    /// <inheritdoc />
    public PrincipalChain<bool> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public BooleanOperationsManager(bool value, string caller)
    {
        PrincipalChain = PrincipalChain<bool>.Get(value, caller);
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
    /// true.Test().Be(true);
    /// </code>
    /// </example>
    public BooleanOperationsManager Be(bool? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Boolean.Be))
        {
            return this;
        }

        ExecutionEngine<BooleanOperationsManager, bool>
            .New(this)
            .WithOperation(BooleanBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BooleanFormatter.Format(PrincipalChain.GetValue()),
                            BooleanFormatter.Format(expected)
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
    public BooleanOperationsManager NotBe(bool? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Boolean.NotBe))
        {
            return this;
        }

        ExecutionEngine<BooleanOperationsManager, bool>
            .New(this)
            .WithOperation(BooleanNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, BooleanFormatter.Format(expected))
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is <c>true</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public BooleanOperationsManager BeTrue(Reason? reason = null)
    {
        return Be(true, reason);
    }

    /// <summary>
    /// Asserts that the value is <c>false</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public BooleanOperationsManager BeFalse(Reason? reason = null)
    {
        return Be(false, reason);
    }

    /// <summary>
    /// Asserts that the value is not <c>true</c> (i.e., is <c>false</c>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public BooleanOperationsManager NotBeTrue(Reason? reason = null)
    {
        return NotBe(true, reason);
    }

    /// <summary>
    /// Asserts that the value is not <c>false</c> (i.e., is <c>true</c>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public BooleanOperationsManager NotBeFalse(Reason? reason = null)
    {
        return NotBe(false, reason);
    }

    /// <summary>
    /// Asserts that all of the provided <paramref name="booleans"/> are <c>true</c>.
    /// </summary>
    /// <param name="booleans">The boolean values to check.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if no arguments are provided or if any argument is <c>null</c>.
    /// </remarks>
    public BooleanOperationsManager BeAllTrue(params bool?[] booleans)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Boolean.BeAllTrue))
        {
            return this;
        }

        ExecutionEngine<BooleanOperationsManager, bool>
            .New(this)
            .WithOperation(BooleanBeAllTrueValidator.New(PrincipalChain, booleans))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
            )
            .FailIf(
                _ =>
                    (
                        booleans.Length == 0,
                        Fail.New(
                            $"The {nameof(BeAllTrue)} operation failed because you have not provided any boolean arguments."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        booleans.Any(x => x is null),
                        Fail.New(
                            $"The {nameof(BeAllTrue)} operation failed because one of the boolean arguments has a <null> value."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that all of the provided <paramref name="booleans"/> are <c>false</c>.
    /// </summary>
    /// <param name="booleans">The boolean values to check.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if no arguments are provided or if any argument is <c>null</c>.
    /// </remarks>
    public BooleanOperationsManager BeAllFalse(params bool?[] booleans)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Boolean.BeAllFalse))
        {
            return this;
        }

        ExecutionEngine<BooleanOperationsManager, bool>
            .New(this)
            .WithOperation(BooleanBeAllFalseValidator.New(PrincipalChain, booleans))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
            )
            .FailIf(
                _ =>
                    (
                        booleans.Length == 0,
                        Fail.New(
                            $"The {nameof(BeAllFalse)} operation failed because you have not provided any boolean arguments."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        booleans.Any(x => x is null),
                        Fail.New(
                            $"The {nameof(BeAllFalse)} operation failed because one of the boolean arguments has a <null> value."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value logically implies <paramref name="consequent"/>
    /// (i.e., if the value is <c>true</c>, then <paramref name="consequent"/> must also be <c>true</c>).
    /// </summary>
    /// <param name="consequent">The boolean value that must be <c>true</c> when the subject is <c>true</c>.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public BooleanOperationsManager Imply(bool consequent, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Boolean.Imply))
        {
            return this;
        }

        ExecutionEngine<BooleanOperationsManager, bool>
            .New(this)
            .WithOperation(BooleanImplyValidator.New(PrincipalChain, consequent))
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
