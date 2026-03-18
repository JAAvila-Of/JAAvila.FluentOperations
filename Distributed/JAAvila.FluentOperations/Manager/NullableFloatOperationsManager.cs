using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>float?</c> values.
/// Supports value presence, equality, comparison, range, precision, approximation, NaN, infinity, and membership validations.
/// </summary>
public class NullableFloatOperationsManager
    : BaseNullableOperationsManager<NullableFloatOperationsManager, float?>,
        ITestManager<NullableFloatOperationsManager, float?>
{
    /// <inheritdoc />
    public PrincipalChain<float?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableFloatOperationsManager(float? value, string caller)
    {
        PrincipalChain = PrincipalChain<float?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableFloatOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableFloat.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatHaveValueValidator.New(PrincipalChain))
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
    public NullableFloatOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableFloat.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatNotHaveValueValidator.New(PrincipalChain))
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
    public NullableFloatOperationsManager Be(float? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.Be))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeValidator.New(PrincipalChain, expected))
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
    public NullableFloatOperationsManager NotBe(float? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatNotBeValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is greater than zero.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BePositive))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBePositiveValidator.New(PrincipalChain))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BePositive)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BePositive)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is less than zero.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeNegative(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeNegative))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeNegativeValidator.New(PrincipalChain))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeNegative)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BeNegative)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is equal to zero.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeZero(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeZero))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeZeroValidator.New(PrincipalChain))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeZero)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BeZero)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is greater than <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The exclusive lower bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeGreaterThan(float value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeGreaterThanValidator.New(PrincipalChain, value))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeGreaterThan)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BeGreaterThan)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is greater than or equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The inclusive lower bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeGreaterThanOrEqualTo(float value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeGreaterThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeGreaterThanOrEqualToValidator.New(PrincipalChain, value))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeGreaterThanOrEqualTo)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BeGreaterThanOrEqualTo)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is less than <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The exclusive upper bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeLessThan(float value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeLessThanValidator.New(PrincipalChain, value))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeLessThan)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BeLessThan)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is less than or equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The inclusive upper bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeLessThanOrEqualTo(float value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeLessThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeLessThanOrEqualToValidator.New(PrincipalChain, value))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeLessThanOrEqualTo)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BeLessThanOrEqualTo)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value falls within the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard).
    /// Also throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public NullableFloatOperationsManager BeInRange(float min, float max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeInRangeValidator.New(PrincipalChain, min, max))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeInRange)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BeInRange)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        min > max,
                        Fail.New(
                            $"The {nameof(BeInRange)} operation failed because min ({min}) is greater than max ({max})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value does not fall within the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard).
    /// Also throws immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range guard).
    /// </remarks>
    public NullableFloatOperationsManager NotBeInRange(float min, float max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatNotBeInRangeValidator.New(PrincipalChain, min, max))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeInRange)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(NotBeInRange)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        min > max,
                        Fail.New(
                            $"The {nameof(NotBeInRange)} operation failed because min ({min}) is greater than max ({max})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is one of the specified <paramref name="values"/>.
    /// </summary>
    /// <param name="values">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard).
    /// Also throws immediately if <paramref name="values"/> is empty (empty set guard).
    /// </remarks>
    public NullableFloatOperationsManager BeOneOf(params float[] values)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeOneOfValidator.New(PrincipalChain, values))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeOneOf)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        values.Length == 0,
                        Fail.New(
                            $"The {nameof(BeOneOf)} operation failed because you have not provided any values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not any of the specified <paramref name="values"/>.
    /// </summary>
    /// <param name="values">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard).
    /// Also throws immediately if <paramref name="values"/> is empty (empty set guard).
    /// </remarks>
    public NullableFloatOperationsManager NotBeOneOf(params float[] values)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatNotBeOneOfValidator.New(PrincipalChain, values))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeOneOf)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        values.Length == 0,
                        Fail.New(
                            $"The {nameof(NotBeOneOf)} operation failed because you have not provided any values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is evenly divisible by <paramref name="divisor"/>.
    /// </summary>
    /// <param name="divisor">The divisor to test against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard).
    /// Also throws immediately if <paramref name="divisor"/> is zero or <c>float.NaN</c> (zero/NaN divisor guard).
    /// </remarks>
    public NullableFloatOperationsManager BeDivisibleBy(float divisor, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeDivisibleBy))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeDivisibleByValidator.New(PrincipalChain, divisor))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeDivisibleBy)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BeDivisibleBy)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        divisor == 0f,
                        Fail.New(
                            $"The {nameof(BeDivisibleBy)} operation failed because divisor cannot be zero."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value has exactly <paramref name="expectedDecimals"/> decimal places.
    /// </summary>
    /// <param name="expectedDecimals">The expected number of decimal places.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager HavePrecision(int expectedDecimals, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.HavePrecision))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(
                NullableFloatHavePrecisionValidator.New(PrincipalChain, expectedDecimals)
            )
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HavePrecision)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(HavePrecision)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is rounded to <paramref name="decimals"/> decimal places
    /// (i.e. has at most that many significant decimal digits).
    /// </summary>
    /// <param name="decimals">The maximum number of decimal places allowed.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeRoundedTo(int decimals, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeRoundedTo))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeRoundedToValidator.New(PrincipalChain, decimals))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeRoundedTo)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BeRoundedTo)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is within <paramref name="tolerance"/> of <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The reference value to compare against.</param>
    /// <param name="tolerance">The maximum allowed absolute difference.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeApproximately(
        float expected,
        float tolerance,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeApproximately))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(
                NullableFloatBeApproximatelyValidator.New(PrincipalChain, expected, tolerance)
            )
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeApproximately)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is not null
                            && float.IsNaN(manager.PrincipalChain.GetValue()!.Value),
                        Fail.New(
                            $"The {nameof(BeApproximately)} operation failed because the value was NaN. "
                            + $"Use {nameof(BeNaN)} or {nameof(NotBeNaN)} to validate NaN values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is <c>float.NaN</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeNaN(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeNaN))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeNaNValidator.New(PrincipalChain))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeNaN)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not <c>float.NaN</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager NotBeNaN(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.NotBeNaN))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatNotBeNaNValidator.New(PrincipalChain))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeNaN)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is <c>float.PositiveInfinity</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BePositiveInfinity(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BePositiveInfinity))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBePositiveInfinityValidator.New(PrincipalChain))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BePositiveInfinity)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is <c>float.NegativeInfinity</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeNegativeInfinity(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeNegativeInfinity))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeNegativeInfinityValidator.New(PrincipalChain))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeNegativeInfinity)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is a finite number (not <c>NaN</c>, not infinity).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> (null guard — use <c>NotBeNull</c> first to cover that scenario).
    /// </remarks>
    public NullableFloatOperationsManager BeFinite(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Float.BeFinite))
        {
            return this;
        }

        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(NullableFloatBeFiniteValidator.New(PrincipalChain))
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
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeFinite)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public NullableFloatOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public NullableFloatOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public NullableFloatOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public NullableFloatOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<float?>.New(PrincipalChain, type!))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        type is null,
                        Fail.New($"The {nameof(BeOfType)} operation failed because the expected type was <null>.")
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New($"The {nameof(BeOfType)} operation failed because the value was <null>.")
                    )
            )
            .Execute();
    }

    private void ValidateNotBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<NullableFloatOperationsManager, float?>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<float?>.New(PrincipalChain, type!))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        type is null,
                        Fail.New($"The {nameof(NotBeOfType)} operation failed because the expected type was <null>.")
                    )
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New($"The {nameof(NotBeOfType)} operation failed because the value was <null>.")
                    )
            )
            .Execute();
    }
}