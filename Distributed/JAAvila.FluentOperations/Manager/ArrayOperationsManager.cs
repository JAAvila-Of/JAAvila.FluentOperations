using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for array/enumerable values.
/// Supports length, content, ordering, and element validations.
/// </summary>
public class ArrayOperationsManager<T> : ITestManager<ArrayOperationsManager<T>, IEnumerable<T>>
{
    /// <inheritdoc />
    public PrincipalChain<IEnumerable<T>> PrincipalChain { get; }
    private readonly T[]? _array;

    /// <summary>
    /// Initializes a new instance for testing the specified array.
    /// </summary>
    /// <param name="value">The array value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public ArrayOperationsManager(T[] value, string caller)
    {
        _array = value;
        PrincipalChain = PrincipalChain<IEnumerable<T>>.Get(_array, caller);
        GlobalConfig.Initialize();
    }

    /// <summary>
    /// Asserts that the array has exactly the specified number of elements.
    /// </summary>
    /// <param name="expected">The expected element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public ArrayOperationsManager<T> HaveLength(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Array.HaveLength))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(ArrayHaveLengthValidator<T>.New(PrincipalChain, _array, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            _array?.Length.ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveLength)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected < 0,
                        Fail.New(
                            $"The {nameof(HaveLength)} operation failed because the expected length cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains more elements than the specified count.
    /// </summary>
    /// <param name="expected">The exclusive lower bound for the element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public ArrayOperationsManager<T> HaveLengthGreaterThan(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Array.HaveLengthGreaterThan))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(
                ArrayHaveLengthGreaterThanValidator<T>.New(PrincipalChain, _array, expected)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            _array?.Length.ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveLengthGreaterThan)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected < 0,
                        Fail.New(
                            $"The {nameof(HaveLengthGreaterThan)} operation failed because the expected length cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains fewer elements than the specified count.
    /// </summary>
    /// <param name="expected">The exclusive upper bound for the element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public ArrayOperationsManager<T> HaveLengthLessThan(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Array.HaveLengthLessThan))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(
                ArrayHaveLengthLessThanValidator<T>.New(PrincipalChain, _array, expected)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            _array?.Length.ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveLengthLessThan)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected < 0,
                        Fail.New(
                            $"The {nameof(HaveLengthLessThan)} operation failed because the expected length cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array is neither <c>null</c> nor empty.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ArrayOperationsManager<T> NotBeNullOrEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.NotBeNullOrEmpty))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionNotBeNullOrEmptyValidator<T>.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            PrincipalChain.GetValue().IsNull() ? "null" : "empty"
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains no elements.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> BeEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeEmpty))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionBeEmptyValidator<T>.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, _array?.Length.ToString() ?? "null")
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(BeEmpty)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains at least one element.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> NotBeEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.NotBeEmpty))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionNotBeEmptyValidator<T>.New(PrincipalChain))
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
                            $"The {nameof(NotBeEmpty)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains exactly the specified number of elements.
    /// </summary>
    /// <param name="expected">The expected element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public ArrayOperationsManager<T> HaveCount(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveCount))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveCountValidator<T>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            _array?.Length.ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveCount)} operation failed because the array was null."
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
    /// Asserts that the array contains more elements than the specified count.
    /// </summary>
    /// <param name="expected">The exclusive lower bound for the element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public ArrayOperationsManager<T> HaveCountGreaterThan(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveCountGreaterThan))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveCountGreaterThanValidator<T>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            _array?.Length.ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveCountGreaterThan)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected < 0,
                        Fail.New(
                            $"The {nameof(HaveCountGreaterThan)} operation failed because the expected count cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains fewer elements than the specified count.
    /// </summary>
    /// <param name="expected">The exclusive upper bound for the element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public ArrayOperationsManager<T> HaveCountLessThan(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveCountLessThan))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveCountLessThanValidator<T>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            _array?.Length.ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveCountLessThan)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        expected < 0,
                        Fail.New(
                            $"The {nameof(HaveCountLessThan)} operation failed because the expected count cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains the specified item.
    /// </summary>
    /// <param name="item">The item expected to be present in the array.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> Contain(T item, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.Contain))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionContainValidator<T>.New(PrincipalChain, item))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, BaseFormatter.Format(item))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array does not contain the specified item.
    /// </summary>
    /// <param name="item">The item expected to be absent from the array.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> NotContain(T item, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.NotContain))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionNotContainValidator<T>.New(PrincipalChain, item))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, BaseFormatter.Format(item))
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(NotContain)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains exactly one element.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> ContainSingle(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainSingle))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionContainSingleValidator<T>.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation, _array?.Length.ToString() ?? "null")
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(ContainSingle)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains all of the specified items.
    /// </summary>
    /// <param name="items">The items that must all be present in the array.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ArrayOperationsManager<T> ContainAll(params T[] items)
    {
        return ContainAll(null, items);
    }

    /// <summary>
    /// Asserts that the array contains all of the specified items.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="items">The items that must all be present in the array.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="items"/> is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> ContainAll(Reason? reason, params T[] items)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainAll))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionContainAllValidator<T>.New(PrincipalChain, items))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", items.Select(i => BaseFormatter.Format(i)))
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(ContainAll)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        items.IsNull(),
                        Fail.New(
                            $"The {nameof(ContainAll)} operation failed because the items parameter cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains at least one of the specified items.
    /// </summary>
    /// <param name="items">The candidate items, at least one of which must be present in the array.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ArrayOperationsManager<T> ContainAny(params T[] items)
    {
        return ContainAny(null, items);
    }

    /// <summary>
    /// Asserts that the array contains at least one of the specified items.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="items">The candidate items, at least one of which must be present in the array.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="items"/> is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> ContainAny(Reason? reason, params T[] items)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainAny))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionContainAnyValidator<T>.New(PrincipalChain, items))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", items.Select(BaseFormatter.Format))
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(ContainAny)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        items.IsNull(),
                        Fail.New(
                            $"The {nameof(ContainAny)} operation failed because the items parameter cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that every element in the array is also present in the specified superset.
    /// </summary>
    /// <param name="superset">The collection that must contain all elements of the tested array.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="superset"/> is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> BeSubsetOf(IEnumerable<T> superset, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeSubsetOf))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionBeSubsetOfValidator<T>.New(PrincipalChain, superset))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", superset.Select(BaseFormatter.Format))
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(BeSubsetOf)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        superset.IsNull(),
                        Fail.New(
                            $"The {nameof(BeSubsetOf)} operation failed because the superset parameter cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that at least one element in the array is not present in the specified superset.
    /// </summary>
    /// <param name="superset">The collection against which subset membership is checked.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="superset"/> is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> NotBeSubsetOf(IEnumerable<T> superset, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.NotBeSubsetOf))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionNotBeSubsetOfValidator<T>.New(PrincipalChain, superset))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", superset.Select(BaseFormatter.Format))
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(NotBeSubsetOf)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        superset.IsNull(),
                        Fail.New(
                            $"The {nameof(NotBeSubsetOf)} operation failed because the superset parameter cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array shares at least one common element with the specified collection.
    /// </summary>
    /// <param name="other">The collection to intersect with.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="other"/> is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> IntersectWith(IEnumerable<T> other, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.IntersectWith))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionIntersectWithValidator<T>.New(PrincipalChain, other))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", other.Select(BaseFormatter.Format))
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(IntersectWith)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        other.IsNull(),
                        Fail.New(
                            $"The {nameof(IntersectWith)} operation failed because the other collection parameter cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array shares no common elements with the specified collection.
    /// </summary>
    /// <param name="other">The collection to test for disjointness.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="other"/> is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> NotIntersectWith(IEnumerable<T> other, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.NotIntersectWith))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionNotIntersectWithValidator<T>.New(PrincipalChain, other))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", other.Select(BaseFormatter.Format))
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(NotIntersectWith)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        other.IsNull(),
                        Fail.New(
                            $"The {nameof(NotIntersectWith)} operation failed because the other collection parameter cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array elements are sorted in ascending order.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// Empty and single-element arrays always pass.
    /// </remarks>
    public ArrayOperationsManager<T> BeInAscendingOrder(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeInAscendingOrder))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionBeInAscendingOrderValidator<T>.New(PrincipalChain))
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
                            $"The {nameof(BeInAscendingOrder)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array elements are sorted in descending order.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// Empty and single-element arrays always pass.
    /// </remarks>
    public ArrayOperationsManager<T> BeInDescendingOrder(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeInDescendingOrder))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionBeInDescendingOrderValidator<T>.New(PrincipalChain))
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
                            $"The {nameof(BeInDescendingOrder)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that every element in the array satisfies the specified predicate.
    /// </summary>
    /// <param name="predicate">A function that must return <c>true</c> for every element.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="predicate"/> is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> AllSatisfy(Func<T, bool> predicate, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.AllSatisfy))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionAllSatisfyValidator<T>.New(PrincipalChain, predicate))
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
                            $"The {nameof(AllSatisfy)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        predicate.IsNull(),
                        Fail.New(
                            $"The {nameof(AllSatisfy)} operation failed because the predicate cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that at least one element in the array satisfies the specified predicate.
    /// </summary>
    /// <param name="predicate">A function that must return <c>true</c> for at least one element.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="predicate"/> is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> AnySatisfy(Func<T, bool> predicate, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.AnySatisfy))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionAnySatisfyValidator<T>.New(PrincipalChain, predicate))
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
                            $"The {nameof(AnySatisfy)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        predicate.IsNull(),
                        Fail.New(
                            $"The {nameof(AnySatisfy)} operation failed because the predicate cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that all elements in the array are distinct (no duplicates).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> BeUnique(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeUnique))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionBeUniqueValidator<T>.New(PrincipalChain))
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
                            $"The {nameof(BeUnique)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains at least one duplicate element.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> ContainDuplicates(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainDuplicates))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionContainDuplicatesValidator<T>.New(PrincipalChain))
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
                            $"The {nameof(ContainDuplicates)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the first element of the array equals the specified item.
    /// </summary>
    /// <param name="item">The expected first element.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> StartWith(T item, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.StartWith))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionStartWithValidator<T>.New(PrincipalChain, item))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(item),
                            FormatterPipeline.Format(
                                PrincipalChain.GetValue() is { } aFirst
                                    ? aFirst.FirstOrDefault()
                                    : null,
                                typeof(T)
                            )
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(StartWith)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the last element of the array equals the specified item.
    /// </summary>
    /// <param name="item">The expected last element.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> EndWith(T item, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.EndWith))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionEndWithValidator<T>.New(PrincipalChain, item))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(item),
                            FormatterPipeline.Format(
                                PrincipalChain.GetValue() is { } aLast
                                    ? aLast.LastOrDefault()
                                    : null,
                                typeof(T)
                            )
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(EndWith)} operation failed because the array was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the element at the specified index in the array equals the expected value.
    /// </summary>
    /// <param name="index">The zero-based index to check. Must be non-negative.</param>
    /// <param name="expected">The expected element at <paramref name="index"/>.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="index"/> is negative.
    /// </remarks>
    public ArrayOperationsManager<T> HaveElementAt(int index, T expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveElementAt))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveElementAtValidator<T>.New(PrincipalChain, index, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            index.ToString(),
                            FormatterPipeline.Format(
                                _array != null ? _array.ElementAtOrDefault(index) : null,
                                typeof(T)
                            )
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        _array.IsNull(),
                        Fail.New(
                            $"The {nameof(HaveElementAt)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        index < 0,
                        Fail.New(
                            $"The {nameof(HaveElementAt)} operation failed because the index was negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains all of the specified items in the given relative order.
    /// </summary>
    /// <param name="items">The items expected to appear in the array in the specified order.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ArrayOperationsManager<T> ContainInOrder(params T[] items)
    {
        return ContainInOrder(null, items);
    }

    /// <summary>
    /// Asserts that the array contains all of the specified items in the given relative order.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="items">The items expected to appear in the array in the specified order.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="items"/> is <c>null</c>.
    /// </remarks>
    public ArrayOperationsManager<T> ContainInOrder(Reason? reason, params T[] items)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainInOrder))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionContainInOrderValidator<T>.New(PrincipalChain, items))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", items.Select(BaseFormatter.Format))
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(ContainInOrder)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        items.IsNull(),
                        Fail.New(
                            $"The {nameof(ContainInOrder)} operation failed because the items parameter cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that each element in the array satisfies the corresponding predicate by position.
    /// </summary>
    /// <param name="predicates">An ordered list of predicates; the element at index <c>i</c> must satisfy <c>predicates[i]</c>.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ArrayOperationsManager<T> SatisfyRespectively(params Func<T, bool>[] predicates)
    {
        return SatisfyRespectively(null, predicates);
    }

    /// <summary>
    /// Asserts that each element in the array satisfies the corresponding predicate by position.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="predicates">An ordered list of predicates; the element at index <c>i</c> must satisfy <c>predicates[i]</c>.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c>, if <paramref name="predicates"/> is <c>null</c>,
    /// or if no predicates are provided for a non-empty array.
    /// </remarks>
    public ArrayOperationsManager<T> SatisfyRespectively(
        Reason? reason,
        params Func<T, bool>[] predicates
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.SatisfyRespectively))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(
                CollectionSatisfyRespectivelyValidator<T>.New(PrincipalChain, predicates)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            _array?.Length.ToString() ?? "null",
                            predicates.Length.ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(SatisfyRespectively)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        predicates.IsNull() || (predicates.Length == 0 && _array?.Length > 0),
                        Fail.New(
                            $"The {nameof(SatisfyRespectively)} operation failed because no predicates were provided."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains at least the specified minimum number of elements.
    /// </summary>
    /// <param name="min">The minimum required element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="min"/> is negative.
    /// </remarks>
    public ArrayOperationsManager<T> HaveMinCount(int min, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveMinCount))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveMinCountValidator<T>.New(PrincipalChain, min))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            min.ToString(),
                            _array?.Length.ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveMinCount)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        min < 0,
                        Fail.New(
                            $"The {nameof(HaveMinCount)} operation failed because the minimum count cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the array contains at most the specified maximum number of elements.
    /// </summary>
    /// <param name="max">The maximum allowed element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the array is <c>null</c> or if <paramref name="max"/> is negative.
    /// </remarks>
    public ArrayOperationsManager<T> HaveMaxCount(int max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveMaxCount))
        {
            return this;
        }

        ExecutionEngine<ArrayOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveMaxCountValidator<T>.New(PrincipalChain, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            max.ToString(),
                            _array?.Length.ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveMaxCount)} operation failed because the array was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        max < 0,
                        Fail.New(
                            $"The {nameof(HaveMaxCount)} operation failed because the maximum count cannot be negative."
                        )
                    )
            )
            .Execute();

        return this;
    }
}
