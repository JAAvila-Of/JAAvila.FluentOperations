using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>ushort</c> values.
/// Supports equality, comparison, range, divisibility, parity, and membership validations.
/// Note: <c>BeNegative()</c> is intentionally excluded because <c>ushort</c> is unsigned (0-65,535).
/// </summary>
public class UShortOperationsManager : ITestManager<UShortOperationsManager, ushort>
{
    /// <inheritdoc />
    public PrincipalChain<ushort> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public UShortOperationsManager(ushort value, string caller)
    {
        PrincipalChain = PrincipalChain<ushort>.Get(value, caller);
        GlobalConfig.Initialize();
    }

    /// <summary>
    /// Asserts that the value is equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public UShortOperationsManager Be(ushort expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.Be))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeValidator.New(PrincipalChain, expected))
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
    public UShortOperationsManager NotBe(ushort expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.NotBe))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortNotBeValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is strictly positive (greater than zero).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public UShortOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BePositive))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBePositiveValidator.New(PrincipalChain))
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
    /// Asserts that the value is equal to zero.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public UShortOperationsManager BeZero(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeZero))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeZeroValidator.New(PrincipalChain))
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
    /// Asserts that the value is greater than <paramref name="expected"/>.
    /// </summary>
    public UShortOperationsManager BeGreaterThan(ushort expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeGreaterThanValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is greater than or equal to <paramref name="expected"/>.
    /// </summary>
    public UShortOperationsManager BeGreaterThanOrEqualTo(ushort expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeGreaterThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeGreaterThanOrEqualToValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is less than <paramref name="expected"/>.
    /// </summary>
    public UShortOperationsManager BeLessThan(ushort expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeLessThanValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is less than or equal to <paramref name="expected"/>.
    /// </summary>
    public UShortOperationsManager BeLessThanOrEqualTo(ushort expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeLessThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeLessThanOrEqualToValidator.New(PrincipalChain, expected))
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
    /// Asserts that the value is within the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <remarks>
    /// Fails immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range).
    /// </remarks>
    public UShortOperationsManager BeInRange(ushort min, ushort max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeInRange))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(min),
                            BaseFormatter.Format(max),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        min > max,
                        Fail.New(
                            $"The {nameof(BeInRange)} operation failed because the range is inverted: min ({min}) > max ({max})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is outside the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <remarks>
    /// Fails immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range).
    /// </remarks>
    public UShortOperationsManager NotBeInRange(ushort min, ushort max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortNotBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(min),
                            BaseFormatter.Format(max),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        min > max,
                        Fail.New(
                            $"The {nameof(NotBeInRange)} operation failed because the range is inverted: min ({min}) > max ({max})."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is one of the specified allowed values.
    /// </summary>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty.
    /// </remarks>
    public UShortOperationsManager BeOneOf(Reason? reason = null, params ushort[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", expected.Select(e => BaseFormatter.Format(e))),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expected is null || expected.Length == 0,
                        Fail.New(
                            $"The {nameof(BeOneOf)} operation failed because you have not provided any values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not one of the specified disallowed values.
    /// </summary>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty.
    /// </remarks>
    public UShortOperationsManager NotBeOneOf(Reason? reason = null, params ushort[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortNotBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", expected.Select(e => BaseFormatter.Format(e))),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expected is null || expected.Length == 0,
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
    /// <remarks>
    /// Fails immediately if <paramref name="divisor"/> is zero.
    /// </remarks>
    public UShortOperationsManager BeDivisibleBy(ushort divisor, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeDivisibleBy))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeDivisibleByValidator.New(PrincipalChain, divisor))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(divisor),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        divisor == 0,
                        Fail.New(
                            $"The {nameof(BeDivisibleBy)} operation failed because the divisor cannot be zero."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is even (divisible by 2).
    /// </summary>
    public UShortOperationsManager BeEven(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeEven))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeEvenValidator.New(PrincipalChain))
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
    /// Asserts that the value is odd (not divisible by 2).
    /// </summary>
    public UShortOperationsManager BeOdd(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.UShort.BeOdd))
        {
            return this;
        }

        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(UShortBeOddValidator.New(PrincipalChain))
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
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public UShortOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public UShortOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public UShortOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public UShortOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<ushort>.New(PrincipalChain, type!))
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
            .Execute();
    }

    private void ValidateNotBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<UShortOperationsManager, ushort>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<ushort>.New(PrincipalChain, type!))
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
            .Execute();
    }
}