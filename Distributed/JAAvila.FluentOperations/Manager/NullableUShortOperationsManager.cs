using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>ushort?</c> values.
/// Supports value presence, equality, comparison, range, divisibility, parity, and membership validations.
/// Note: <c>BeNegative()</c> is intentionally excluded because <c>ushort</c> is unsigned (0-65,535).
/// </summary>
public class NullableUShortOperationsManager
    : BaseNullableOperationsManager<NullableUShortOperationsManager, ushort?>,
        ITestManager<NullableUShortOperationsManager, ushort?>
{
    /// <inheritdoc />
    public PrincipalChain<ushort?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableUShortOperationsManager(ushort? value, string caller)
    {
        PrincipalChain = PrincipalChain<ushort?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    public NullableUShortOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableUShort.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortHaveValueValidator.New(PrincipalChain))
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
    public NullableUShortOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableUShort.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortNotHaveValueValidator.New(PrincipalChain))
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
    public NullableUShortOperationsManager Be(ushort? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.Be))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortBeValidator.New(PrincipalChain, expected))
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
    public NullableUShortOperationsManager NotBe(ushort? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortNotBeValidator.New(PrincipalChain, expected))
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
    public NullableUShortOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BePositive))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortBePositiveValidator.New(PrincipalChain))
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
    public NullableUShortOperationsManager BeZero(Reason? reason = null)
    {
        return Be(0, reason);
    }

    /// <summary>
    /// Asserts that the value is greater than <paramref name="value"/>.
    /// </summary>
    public NullableUShortOperationsManager BeGreaterThan(ushort value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortBeGreaterThanValidator.New(PrincipalChain, value))
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
    public NullableUShortOperationsManager BeGreaterThanOrEqualTo(ushort value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeGreaterThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(
                NullableUShortBeGreaterThanOrEqualToValidator.New(PrincipalChain, value)
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
    public NullableUShortOperationsManager BeLessThan(ushort value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortBeLessThanValidator.New(PrincipalChain, value))
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
    public NullableUShortOperationsManager BeLessThanOrEqualTo(ushort value, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeLessThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortBeLessThanOrEqualToValidator.New(PrincipalChain, value))
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
    public NullableUShortOperationsManager BeInRange(ushort min, ushort max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortBeInRangeValidator.New(PrincipalChain, min, max))
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
    public NullableUShortOperationsManager NotBeInRange(ushort min, ushort max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortNotBeInRangeValidator.New(PrincipalChain, min, max))
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
    public NullableUShortOperationsManager BeOneOf(params ushort[] values)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortBeOneOfValidator.New(PrincipalChain, values))
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
    public NullableUShortOperationsManager NotBeOneOf(params ushort[] values)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortNotBeOneOfValidator.New(PrincipalChain, values))
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
    public NullableUShortOperationsManager BeDivisibleBy(ushort divisor, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeDivisibleBy))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortBeDivisibleByValidator.New(PrincipalChain, divisor))
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
    public NullableUShortOperationsManager BeEven(Reason? reason = null)
    {
        return BeDivisibleBy(2, reason);
    }

    /// <summary>
    /// Asserts that the value is an odd number (not divisible by 2).
    /// </summary>
    public NullableUShortOperationsManager BeOdd(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeOdd))
        {
            return this;
        }

        ExecutionEngine<NullableUShortOperationsManager, ushort?>
            .New(this)
            .WithOperation(NullableUShortBeOddValidator.New(PrincipalChain))
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
}
