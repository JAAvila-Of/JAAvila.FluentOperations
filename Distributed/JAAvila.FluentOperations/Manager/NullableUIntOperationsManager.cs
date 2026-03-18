using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>uint?</c> values.
/// Supports value presence, equality, comparison, range, divisibility, parity, and membership validations.
/// Note: <c>BeNegative()</c> is intentionally excluded because <c>uint</c> is unsigned (0-4,294,967,295).
/// </summary>
public class NullableUIntOperationsManager
    : BaseNullableOperationsManager<NullableUIntOperationsManager, uint?>,
        ITestManager<NullableUIntOperationsManager, uint?>
{
    /// <inheritdoc />
    public PrincipalChain<uint?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableUIntOperationsManager(uint? value, string caller)
    {
        PrincipalChain = PrincipalChain<uint?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableUIntOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableUInt.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntHaveValueValidator.New(PrincipalChain))
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
    public NullableUIntOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableUInt.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntNotHaveValueValidator.New(PrincipalChain))
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
    public NullableUIntOperationsManager Be(uint? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.Be))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntBeValidator.New(PrincipalChain, expected))
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
    public NullableUIntOperationsManager NotBe(uint? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntNotBeValidator.New(PrincipalChain, expected))
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
    public NullableUIntOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.BePositive))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntBePositiveValidator.New(PrincipalChain))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is equal to zero.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableUIntOperationsManager BeZero(Reason? reason = null)
    {
        return Be(0, reason);
    }

    /// <summary>
    /// Asserts that the value is greater than <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The exclusive lower bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableUIntOperationsManager BeGreaterThan(uint value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntBeGreaterThanValidator.New(PrincipalChain, value))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is greater than or equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The inclusive lower bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableUIntOperationsManager BeGreaterThanOrEqualTo(uint value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.BeGreaterThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(
                NullableUIntBeGreaterThanOrEqualToValidator.New(PrincipalChain, value)
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
                            $"The {nameof(BeGreaterThanOrEqualTo)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
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
    public NullableUIntOperationsManager BeLessThan(uint value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntBeLessThanValidator.New(PrincipalChain, value))
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
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is less than or equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The inclusive upper bound.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableUIntOperationsManager BeLessThanOrEqualTo(uint value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.BeLessThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntBeLessThanOrEqualToValidator.New(PrincipalChain, value))
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
    public NullableUIntOperationsManager BeInRange(uint min, uint max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.BeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntBeInRangeValidator.New(PrincipalChain, min, max))
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
    public NullableUIntOperationsManager NotBeInRange(uint min, uint max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntNotBeInRangeValidator.New(PrincipalChain, min, max))
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
    public NullableUIntOperationsManager BeOneOf(params uint[] values)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntBeOneOfValidator.New(PrincipalChain, values))
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
    public NullableUIntOperationsManager NotBeOneOf(params uint[] values)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntNotBeOneOfValidator.New(PrincipalChain, values))
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
    public NullableUIntOperationsManager BeDivisibleBy(uint divisor, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.BeDivisibleBy))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntBeDivisibleByValidator.New(PrincipalChain, divisor))
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
                _ =>
                    (
                        divisor == 0,
                        Fail.New(
                            $"The {nameof(BeDivisibleBy)} operation failed because divisor cannot be zero."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is an even number (divisible by 2).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableUIntOperationsManager BeEven(Reason? reason = null)
    {
        return BeDivisibleBy(2, reason);
    }

    /// <summary>
    /// Asserts that the value is an odd number (not divisible by 2).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableUIntOperationsManager BeOdd(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UInt.BeOdd))
        {
            return this;
        }

        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(NullableUIntBeOddValidator.New(PrincipalChain))
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
                            $"The {nameof(BeOdd)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public NullableUIntOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public NullableUIntOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public NullableUIntOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public NullableUIntOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<uint?>.New(PrincipalChain, type!))
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
        ExecutionEngine<NullableUIntOperationsManager, uint?>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<uint?>.New(PrincipalChain, type!))
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