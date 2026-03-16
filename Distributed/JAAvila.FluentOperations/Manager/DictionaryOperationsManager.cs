using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Connector;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for dictionary values.
/// Supports key/value presence, count, and content validations.
/// </summary>
public class DictionaryOperationsManager<TKey, TValue>
    : ITestManager<DictionaryOperationsManager<TKey, TValue>, IDictionary<TKey, TValue>>
    where TKey : notnull
{
    /// <inheritdoc />
    public PrincipalChain<IDictionary<TKey, TValue>> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified dictionary.
    /// </summary>
    /// <param name="value">The dictionary value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public DictionaryOperationsManager(IDictionary<TKey, TValue> value, string caller)
    {
        PrincipalChain = PrincipalChain<IDictionary<TKey, TValue>>.Get(value, caller);
        GlobalConfig.Initialize();
    }

    /// <summary>
    /// Asserts that the dictionary contains an entry with the specified key.
    /// </summary>
    /// <param name="key">The key expected to be present in the dictionary.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the dictionary is <c>null</c>.
    /// </remarks>
    public DictionaryOperationsManager<TKey, TValue> ContainKey(TKey key, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Dictionary.ContainKey))
        {
            return this;
        }

        ExecutionEngine<DictionaryOperationsManager<TKey, TValue>, IDictionary<TKey, TValue>>
            .New(this)
            .WithOperation(DictionaryContainKeyValidator<TKey, TValue>.New(PrincipalChain, key))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, BaseFormatter.Format(key))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(ContainKey)} operation failed because the dictionary was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the dictionary does not contain an entry with the specified key.
    /// </summary>
    /// <param name="key">The key expected to be absent from the dictionary.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the dictionary is <c>null</c>.
    /// </remarks>
    public DictionaryOperationsManager<TKey, TValue> NotContainKey(TKey key, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Dictionary.NotContainKey))
        {
            return this;
        }

        ExecutionEngine<DictionaryOperationsManager<TKey, TValue>, IDictionary<TKey, TValue>>
            .New(this)
            .WithOperation(DictionaryNotContainKeyValidator<TKey, TValue>.New(PrincipalChain, key))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, BaseFormatter.Format(key))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(NotContainKey)} operation failed because the dictionary was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the dictionary contains at least one entry with the specified value.
    /// </summary>
    /// <param name="value">The value expected to be present in the dictionary.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the dictionary is <c>null</c>.
    /// </remarks>
    public DictionaryOperationsManager<TKey, TValue> ContainValue(
        TValue value,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Dictionary.ContainValue))
        {
            return this;
        }

        ExecutionEngine<DictionaryOperationsManager<TKey, TValue>, IDictionary<TKey, TValue>>
            .New(this)
            .WithOperation(DictionaryContainValueValidator<TKey, TValue>.New(PrincipalChain, value))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, BaseFormatter.Format(value))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(ContainValue)} operation failed because the dictionary was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the dictionary does not contain any entry with the specified value.
    /// </summary>
    /// <param name="value">The value expected to be absent from the dictionary.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the dictionary is <c>null</c>.
    /// </remarks>
    public DictionaryOperationsManager<TKey, TValue> NotContainValue(
        TValue value,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Dictionary.NotContainValue))
        {
            return this;
        }

        ExecutionEngine<DictionaryOperationsManager<TKey, TValue>, IDictionary<TKey, TValue>>
            .New(this)
            .WithOperation(
                DictionaryNotContainValueValidator<TKey, TValue>.New(PrincipalChain, value)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, BaseFormatter.Format(value))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(NotContainValue)} operation failed because the dictionary was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the dictionary contains an entry with both the specified key and value.
    /// </summary>
    /// <param name="key">The key of the expected key-value pair.</param>
    /// <param name="value">The value of the expected key-value pair.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the dictionary is <c>null</c>.
    /// </remarks>
    public DictionaryOperationsManager<TKey, TValue> ContainPair(
        TKey key,
        TValue value,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Dictionary.ContainPair))
        {
            return this;
        }

        ExecutionEngine<DictionaryOperationsManager<TKey, TValue>, IDictionary<TKey, TValue>>
            .New(this)
            .WithOperation(
                DictionaryContainPairValidator<TKey, TValue>.New(PrincipalChain, key, value)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(key),
                            BaseFormatter.Format(value)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(ContainPair)} operation failed because the dictionary was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the dictionary contains exactly the specified number of entries.
    /// </summary>
    /// <param name="expected">The expected number of entries. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the dictionary is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public DictionaryOperationsManager<TKey, TValue> HaveCount(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Dictionary.HaveCount))
        {
            return this;
        }

        ExecutionEngine<DictionaryOperationsManager<TKey, TValue>, IDictionary<TKey, TValue>>
            .New(this)
            .WithOperation(DictionaryHaveCountValidator<TKey, TValue>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            PrincipalChain.GetValue().Count.ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveCount)} operation failed because the dictionary was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected < 0,
                        Fail.New(
                            $"The {nameof(HaveCount)} operation failed because the expected count cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the dictionary contains no entries.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the dictionary is <c>null</c>.
    /// </remarks>
    public DictionaryOperationsManager<TKey, TValue> BeEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Dictionary.BeEmpty))
        {
            return this;
        }

        ExecutionEngine<DictionaryOperationsManager<TKey, TValue>, IDictionary<TKey, TValue>>
            .New(this)
            .WithOperation(DictionaryBeEmptyValidator<TKey, TValue>.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            PrincipalChain.GetValue().Count.ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(BeEmpty)} operation failed because the dictionary was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the dictionary contains at least one entry.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the dictionary is <c>null</c>.
    /// </remarks>
    public DictionaryOperationsManager<TKey, TValue> NotBeEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Dictionary.NotBeEmpty))
        {
            return this;
        }

        ExecutionEngine<DictionaryOperationsManager<TKey, TValue>, IDictionary<TKey, TValue>>
            .New(this)
            .WithOperation(DictionaryNotBeEmptyValidator<TKey, TValue>.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(NotBeEmpty)} operation failed because the dictionary was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Extracts a sub-value from the current dictionary using the given selector.
    /// Returns a connector that exposes the sub-value for further assertions.
    /// </summary>
    /// <param name="selector">A function that extracts a sub-value from the current dictionary.</param>
    /// <typeparam name="TResult">The type of the extracted sub-value.</typeparam>
    /// <returns>
    /// An <see cref="AndWhichConnector{TManager,TSubject}"/> exposing the extracted sub-value
    /// and allowing the chain to continue from the parent manager via <c>.And</c>.
    /// </returns>
    public AndWhichConnector<DictionaryOperationsManager<TKey, TValue>, TResult> Which<TResult>(
        Func<IDictionary<TKey, TValue>, TResult> selector
    )
    {
        ArgumentNullException.ThrowIfNull(selector);
        var value = PrincipalChain.GetValue();
        var result = selector(value);
        return new AndWhichConnector<DictionaryOperationsManager<TKey, TValue>, TResult>(
            this,
            result,
            PrincipalChain.GetSubject()
        );
    }
}
