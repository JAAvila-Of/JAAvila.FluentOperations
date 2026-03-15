using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Connector;
using JAAvila.FluentOperations.Constraints;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for collection values (<see cref="IEnumerable{T}"/>).
/// Supports count, containment, ordering, subset, intersection, and predicate-based validations.
/// </summary>
public class CollectionOperationsManager<T>
    : ITestManager<CollectionOperationsManager<T>, IEnumerable<T>>
{
    /// <inheritdoc />
    public PrincipalChain<IEnumerable<T>> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified collection.
    /// </summary>
    /// <param name="value">The collection value to test. Materialized eagerly to avoid multiple enumeration. Null is accepted and deferred to FailIf guards.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public CollectionOperationsManager(IEnumerable<T> value, string caller)
    {
        var materialized = value switch
                           {
                               null => null,
                               ICollection<T> col => col.ToList(),
                               _ => value.ToList()
                           };
        PrincipalChain = PrincipalChain<IEnumerable<T>>.Get(materialized!, caller);
        GlobalConfig.Initialize();
    }

    /// <summary>
    /// Asserts that the collection is neither <c>null</c> nor empty.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CollectionOperationsManager<T> NotBeNullOrEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.NotBeNullOrEmpty))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
    /// Asserts that the collection contains no elements.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> BeEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeEmpty))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionBeEmptyValidator<T>.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            PrincipalChain.GetValue()?.Count().ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(BeEmpty)} operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection contains at least one element.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CollectionOperationsManager<T> NotBeEmpty(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.NotBeEmpty))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionNotBeEmptyValidator<T>.New(PrincipalChain))
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
    /// Asserts that the collection contains exactly the specified number of elements.
    /// </summary>
    /// <param name="expected">The expected element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public CollectionOperationsManager<T> HaveCount(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveCount))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveCountValidator<T>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            PrincipalChain.GetValue()?.Count().ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveCount)} operation failed because the collection was null."
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
    /// Asserts that the collection contains more than the specified number of elements.
    /// </summary>
    /// <param name="expected">The exclusive lower bound for the element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public CollectionOperationsManager<T> HaveCountGreaterThan(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveCountGreaterThan))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveCountGreaterThanValidator<T>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            PrincipalChain.GetValue()?.Count().ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveCountGreaterThan)} operation failed because the collection was null."
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
    /// Asserts that the collection contains fewer elements than the specified count.
    /// </summary>
    /// <param name="expected">The exclusive upper bound for the element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="expected"/> is negative.
    /// </remarks>
    public CollectionOperationsManager<T> HaveCountLessThan(int expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveCountLessThan))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveCountLessThanValidator<T>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.ToString(),
                            PrincipalChain.GetValue()?.Count().ToString() ?? "null"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveCountLessThan)} operation failed because the collection was null."
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
    /// Asserts that the collection contains the specified item.
    /// </summary>
    /// <param name="item">The item expected to be present in the collection.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> Contain(T item, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.Contain))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(Contain)} operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection contains at least one element matching the specified predicate.
    /// </summary>
    /// <param name="predicate">A function that must return <c>true</c> for at least one element.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CollectionOperationsManager<T> Contain(Func<T, bool> predicate, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainMatch))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionContainPredicateValidator<T>.New(PrincipalChain, predicate))
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
                            $"The {nameof(Contain)} operation failed because the collection was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        predicate is null,
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because the predicate cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection contains the specified item a number of times satisfying the given occurrence constraint.
    /// </summary>
    /// <param name="item">The item to look for in the collection.</param>
    /// <param name="constraint">The occurrence constraint defining the expected count (e.g., exactly N, at least N).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="constraint"/> is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> Contain(
        T item,
        OccurrenceConstraint constraint,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainWithOccurrence))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(
                CollectionContainWithOccurrenceValidator<T>.New(PrincipalChain, item, constraint)
            )
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
                            $"The {nameof(Contain)} operation failed because the collection was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        constraint == null!,
                        Fail.New(
                            $"The {nameof(Contain)} operation failed because the occurrence constraint was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection does not contain the specified item.
    /// </summary>
    /// <param name="item">The item expected to be absent from the collection.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> NotContain(T item, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.NotContain))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(NotContain)} operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection contains exactly one element.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> ContainSingle(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainSingle))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionContainSingleValidator<T>.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            PrincipalChain.GetValue().Count().ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(ContainSingle)} operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection contains exactly one element matching the specified predicate.
    /// </summary>
    /// <param name="predicate">A function that must return <c>true</c> for exactly one element.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CollectionOperationsManager<T> ContainSingle(Func<T, bool> predicate, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainSingleMatch))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionContainSinglePredicateValidator<T>.New(PrincipalChain, predicate))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            PrincipalChain.GetValue()?.Count(predicate).ToString() ?? "0"
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(ContainSingle)} operation failed because the collection was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        predicate is null,
                        Fail.New(
                            $"The {nameof(ContainSingle)} operation failed because the predicate cannot be null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection contains all of the specified items.
    /// </summary>
    /// <param name="items">The items that must all be present in the collection.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CollectionOperationsManager<T> ContainAll(params T[] items)
    {
        return ContainAll(null, items);
    }

    /// <summary>
    /// Asserts that the collection contains all of the specified items.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="items">The items that must all be present in the collection.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="items"/> is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> ContainAll(Reason? reason, params T[] items)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainAll))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionContainAllValidator<T>.New(PrincipalChain, items))
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
                            $"The {nameof(ContainAll)} operation failed because the collection was null."
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
    /// Asserts that the collection contains at least one of the specified items.
    /// </summary>
    /// <param name="items">The candidate items, at least one of which must be present in the collection.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CollectionOperationsManager<T> ContainAny(params T[] items)
    {
        return ContainAny(null, items);
    }

    /// <summary>
    /// Asserts that the collection contains at least one of the specified items.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="items">The candidate items, at least one of which must be present in the collection.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="items"/> is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> ContainAny(Reason? reason, params T[] items)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainAny))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(ContainAny)} operation failed because the collection was null."
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
    /// Asserts that every element in the collection is also present in the specified superset.
    /// </summary>
    /// <param name="superset">The collection that must contain all elements of the tested collection.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="superset"/> is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> BeSubsetOf(IEnumerable<T> superset, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeSubsetOf))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(BeSubsetOf)} operation failed because the collection was null."
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
    /// Asserts that at least one element in the collection is not present in the specified superset.
    /// </summary>
    /// <param name="superset">The collection against which subset membership is checked.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="superset"/> is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> NotBeSubsetOf(
        IEnumerable<T> superset,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.NotBeSubsetOf))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(NotBeSubsetOf)} operation failed because the collection was null."
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
    /// Asserts that the collection shares at least one common element with the specified collection.
    /// </summary>
    /// <param name="other">The collection to intersect with.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="other"/> is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> IntersectWith(IEnumerable<T> other, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.IntersectWith))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(IntersectWith)} operation failed because the collection was null."
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
    /// Asserts that the collection shares no common elements with the specified collection.
    /// </summary>
    /// <param name="other">The collection to test for disjointness.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="other"/> is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> NotIntersectWith(
        IEnumerable<T> other,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.NotIntersectWith))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(NotIntersectWith)} operation failed because the collection was null."
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
    /// Asserts that the collection elements are sorted in ascending order.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>.
    /// Empty and single-element collections always pass.
    /// </remarks>
    public CollectionOperationsManager<T> BeInAscendingOrder(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeInAscendingOrder))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(BeInAscendingOrder)} operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection elements are sorted in descending order.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>.
    /// Empty and single-element collections always pass.
    /// </remarks>
    public CollectionOperationsManager<T> BeInDescendingOrder(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeInDescendingOrder))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(BeInDescendingOrder)} operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that every element in the collection satisfies the specified predicate.
    /// </summary>
    /// <param name="predicate">A function that must return <c>true</c> for every element.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="predicate"/> is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> AllSatisfy(Func<T, bool> predicate, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.AllSatisfy))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(AllSatisfy)} operation failed because the collection was null."
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
    /// Asserts that at least one element in the collection satisfies the specified predicate.
    /// </summary>
    /// <param name="predicate">A function that must return <c>true</c> for at least one element.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="predicate"/> is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> AnySatisfy(Func<T, bool> predicate, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.AnySatisfy))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(AnySatisfy)} operation failed because the collection was null."
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
    /// Asserts that all elements in the collection are distinct (no duplicates).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> BeUnique(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeUnique))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(BeUnique)} operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection contains at least one duplicate element.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> ContainDuplicates(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainDuplicates))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(ContainDuplicates)} operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the first element of the collection equals the specified item.
    /// </summary>
    /// <param name="item">The expected first element.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> StartWith(T item, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.StartWith))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                                PrincipalChain.GetValue() is { } cFirst
                                    ? cFirst.FirstOrDefault()
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
                            $"The {nameof(StartWith)} operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the last element of the collection equals the specified item.
    /// </summary>
    /// <param name="item">The expected last element.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> EndWith(T item, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.EndWith))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                                PrincipalChain.GetValue() is { } cLast
                                    ? cLast.LastOrDefault()
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
                            $"The {nameof(EndWith)} operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection contains the same elements as the expected collection,
    /// regardless of order.
    /// </summary>
    /// <param name="expected">The collection whose elements must all be present in the tested collection, in any order.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> and <paramref name="expected"/> is not <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> BeEquivalentTo(
        IEnumerable<T> expected,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeEquivalentTo))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionBeEquivalentToValidator<T>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.Count().ToString(),
                            PrincipalChain.GetValue().Count().ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull() && !expected.IsNull(),
                        Fail.New(
                            "The BeEquivalentTo operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection contains the same elements in the same order
    /// as the expected collection.
    /// </summary>
    /// <param name="expected">The collection whose elements must match the tested collection in exact order.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> and <paramref name="expected"/> is not <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> BeSequenceEqualTo(
        IEnumerable<T> expected,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.BeSequenceEqualTo))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionBeSequenceEqualToValidator<T>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            expected.Count().ToString(),
                            PrincipalChain.GetValue().Count().ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull() && !expected.IsNull(),
                        Fail.New(
                            "The BeSequenceEqualTo operation failed because the collection was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the element at the specified index in the collection equals the expected value.
    /// </summary>
    /// <param name="index">The zero-based index to check. Must be non-negative.</param>
    /// <param name="expected">The expected element at <paramref name="index"/>.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="index"/> is negative.
    /// </remarks>
    public CollectionOperationsManager<T> HaveElementAt(
        int index,
        T expected,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveElementAt))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                                PrincipalChain.GetValue() is { } cAt
                                    ? cAt.ElementAtOrDefault(index)
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
                            $"The {nameof(HaveElementAt)} operation failed because the collection was null."
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
    /// Asserts that the collection contains all of the specified items in the given relative order.
    /// </summary>
    /// <param name="items">The items expected to appear in the collection in the specified order.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CollectionOperationsManager<T> ContainInOrder(params T[] items)
    {
        return ContainInOrder(null, items);
    }

    /// <summary>
    /// Asserts that the collection contains all of the specified items in the given relative order.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="items">The items expected to appear in the collection in the specified order.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="items"/> is <c>null</c>.
    /// </remarks>
    public CollectionOperationsManager<T> ContainInOrder(Reason? reason, params T[] items)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.ContainInOrder))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            $"The {nameof(ContainInOrder)} operation failed because the collection was null."
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
    /// Asserts that each element in the collection satisfies the corresponding predicate by position.
    /// </summary>
    /// <param name="predicates">An ordered list of predicates; the element at index <c>i</c> must satisfy <c>predicates[i]</c>.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public CollectionOperationsManager<T> SatisfyRespectively(params Func<T, bool>[] predicates)
    {
        return SatisfyRespectively(null, predicates);
    }

    /// <summary>
    /// Asserts that each element in the collection satisfies the corresponding predicate by position.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="predicates">An ordered list of predicates; the element at index <c>i</c> must satisfy <c>predicates[i]</c>.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c>, if <paramref name="predicates"/> is <c>null</c>,
    /// or if no predicates are provided for a non-empty collection.
    /// </remarks>
    public CollectionOperationsManager<T> SatisfyRespectively(
        Reason? reason,
        params Func<T, bool>[] predicates
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.SatisfyRespectively))
        {
            return this;
        }

        var list = PrincipalChain.GetValue().ToList();

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
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
                            list.Count.ToString(),
                            predicates.Length.ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(SatisfyRespectively)} operation failed because the collection was null."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        predicates.IsNull()
                            || (predicates.Length == 0 && PrincipalChain.GetValue().Any()),
                        Fail.New(
                            $"The {nameof(SatisfyRespectively)} operation failed because no predicates were provided."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the collection contains at least the specified minimum number of elements.
    /// </summary>
    /// <param name="min">The minimum required element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="min"/> is negative.
    /// </remarks>
    public CollectionOperationsManager<T> HaveMinCount(int min, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveMinCount))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveMinCountValidator<T>.New(PrincipalChain, min))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            min.ToString(),
                            PrincipalChain.GetValue().Count().ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveMinCount)} operation failed because the collection was null."
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
    /// Asserts that the collection contains at most the specified maximum number of elements.
    /// </summary>
    /// <param name="max">The maximum allowed element count. Must be non-negative.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// This operation fails immediately if the collection is <c>null</c> or if <paramref name="max"/> is negative.
    /// </remarks>
    public CollectionOperationsManager<T> HaveMaxCount(int max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Collection.HaveMaxCount))
        {
            return this;
        }

        ExecutionEngine<CollectionOperationsManager<T>, IEnumerable<T>>
            .New(this)
            .WithOperation(CollectionHaveMaxCountValidator<T>.New(PrincipalChain, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            max.ToString(),
                            PrincipalChain.GetValue().Count().ToString()
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue().IsNull(),
                        Fail.New(
                            $"The {nameof(HaveMaxCount)} operation failed because the collection was null."
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

    /// <summary>
    /// Extracts a sub-value from the current collection using the given selector.
    /// Returns a connector that exposes the sub-value for further assertions.
    /// </summary>
    /// <param name="selector">A function that extracts a sub-value from the current collection.</param>
    /// <typeparam name="TResult">The type of the extracted sub-value.</typeparam>
    /// <returns>
    /// An <see cref="AndWhichConnector{TManager,TSubject}"/> exposing the extracted sub-value
    /// and allowing the chain to continue from the parent manager via <c>.And</c>.
    /// </returns>
    public AndWhichConnector<CollectionOperationsManager<T>, TResult> Which<TResult>(
        Func<IEnumerable<T>, TResult> selector
    )
    {
        ArgumentNullException.ThrowIfNull(selector);
        var value = PrincipalChain.GetValue();
        var result = selector(value);
        return new AndWhichConnector<CollectionOperationsManager<T>, TResult>(
            this,
            result,
            PrincipalChain.GetSubject()
        );
    }
}
