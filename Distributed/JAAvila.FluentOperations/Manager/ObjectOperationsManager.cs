using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Comparators;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Connector;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>object?</c> values.
/// Supports structural equivalence, deep comparison, reference checks, type checks, and value extraction.
/// </summary>
public class ObjectOperationsManager
    : BaseOperationsManager<ObjectOperationsManager, object?>,
        ITestManager<ObjectOperationsManager, object?>
{
    /// <inheritdoc />
    public PrincipalChain<object?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public ObjectOperationsManager(object? value, string caller)
    {
        PrincipalChain = PrincipalChain<object?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the object is structurally equivalent to <paramref name="expected"/>
    /// using deep property-by-property comparison.
    /// </summary>
    /// <param name="expected">The expected object to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ObjectOperationsManager BeEquivalentTo(object? expected, Reason? reason = null)
    {
        return BeEquivalentTo(expected, ComparisonOptions.Default, reason);
    }

    /// <summary>
    /// Asserts that the object is structurally equivalent to <paramref name="expected"/>
    /// using deep property-by-property comparison with custom <paramref name="options"/>.
    /// </summary>
    /// <param name="expected">The expected object to compare against.</param>
    /// <param name="options">Options controlling the comparison behaviour (depth, max differences, etc.).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ObjectOperationsManager BeEquivalentTo(
        object? expected,
        ComparisonOptions options,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeEquivalentTo))
        {
            return this;
        }

        var result = ObjectComparator.DeepCompare(PrincipalChain.GetValue(), expected, options);

        ExecutionEngine<ObjectOperationsManager, object?>
            .New(this)
            .WithOperation(ObjectBeEquivalentToValidator.New(PrincipalChain, expected, options))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            result.DifferenceDescription ?? "no differences"
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the object is NOT structurally equivalent to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected object to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ObjectOperationsManager NotBeEquivalentTo(object? expected, Reason? reason = null)
    {
        return NotBeEquivalentTo(expected, ComparisonOptions.Default, reason);
    }

    /// <summary>
    /// Asserts that the object is NOT structurally equivalent to <paramref name="expected"/>
    /// using deep property-by-property comparison with custom <paramref name="options"/>.
    /// </summary>
    /// <param name="expected">The expected object to compare against.</param>
    /// <param name="options">Options controlling the comparison behaviour.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ObjectOperationsManager NotBeEquivalentTo(
        object? expected,
        ComparisonOptions options,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeEquivalentTo))
        {
            return this;
        }

        ExecutionEngine<ObjectOperationsManager, object?>
            .New(this)
            .WithOperation(ObjectNotBeEquivalentToValidator.New(PrincipalChain, expected, options))
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
    /// Asserts that the object equals <paramref name="expected"/> using <see cref="object.Equals(object?, object?)"/>.
    /// </summary>
    /// <param name="expected">The expected value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ObjectOperationsManager Be(object? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.Be))
        {
            return this;
        }

        ExecutionEngine<ObjectOperationsManager, object?>
            .New(this)
            .WithOperation(ObjectBeValidator.New(PrincipalChain, expected))
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
    /// Asserts that the object does not equal <paramref name="expected"/> using <see cref="object.Equals(object?, object?)"/>.
    /// </summary>
    /// <param name="expected">The value that should not match.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ObjectOperationsManager NotBe(object? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBe))
        {
            return this;
        }

        ExecutionEngine<ObjectOperationsManager, object?>
            .New(this)
            .WithOperation(ObjectNotBeValidator.New(PrincipalChain, expected))
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
    /// Extracts a sub-value from the current object using the given <paramref name="selector"/>.
    /// Returns a connector that exposes the extracted value for further assertions.
    /// </summary>
    /// <typeparam name="TResult">The type of the extracted value.</typeparam>
    /// <param name="selector">A function that extracts a sub-value from the object.</param>
    /// <returns>An <see cref="AndWhichConnector{TManager,TSubject}"/> for asserting on the extracted value.</returns>
    public AndWhichConnector<ObjectOperationsManager, TResult> Which<TResult>(
        Func<object?, TResult> selector
    )
    {
        ArgumentNullException.ThrowIfNull(selector);
        var value = PrincipalChain.GetValue();
        var result = selector(value);
        return new AndWhichConnector<ObjectOperationsManager, TResult>(
            this,
            result,
            PrincipalChain.GetSubject()
        );
    }
}
