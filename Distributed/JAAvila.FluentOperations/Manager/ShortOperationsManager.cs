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
/// Provides fluent validation operations for <c>short</c> values.
/// Supports equality, comparison, range, divisibility, parity, sign, and membership validations.
/// Unlike <c>ushort</c>, <c>BeNegative()</c> is included because <c>short</c> is signed (-32,768 to 32,767).
/// </summary>
public class ShortOperationsManager : ITestManager<ShortOperationsManager, short>
{
    /// <inheritdoc />
    public PrincipalChain<short> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public ShortOperationsManager(short value, string caller)
    {
        PrincipalChain = PrincipalChain<short>.Get(value, caller);
        GlobalConfig.Initialize();
    }

    /// <summary>
    /// Asserts that the value is equal to <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ShortOperationsManager Be(short expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.Be))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    public ShortOperationsManager NotBe(short expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.NotBe))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(expected)
                        )
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
    public ShortOperationsManager BePositive(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BePositive))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBePositiveValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is strictly negative (less than zero).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ShortOperationsManager BeNegative(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeNegative))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeNegativeValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    public ShortOperationsManager BeZero(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeZero))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeZeroValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ShortOperationsManager BeGreaterThan(short expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeGreaterThan))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeGreaterThanValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ShortOperationsManager BeGreaterThanOrEqualTo(short expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeGreaterThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeGreaterThanOrEqualToValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ShortOperationsManager BeLessThan(short expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeLessThan))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeLessThanValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    /// <param name="expected">The value to compare against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ShortOperationsManager BeLessThanOrEqualTo(short expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeLessThanOrEqualTo))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeLessThanOrEqualToValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range).
    /// </remarks>
    public ShortOperationsManager BeInRange(short min, short max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeInRange))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    /// <param name="min">The lower bound of the range (inclusive).</param>
    /// <param name="max">The upper bound of the range (inclusive).</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="min"/> is greater than <paramref name="max"/> (inverted range).
    /// </remarks>
    public ShortOperationsManager NotBeInRange(short min, short max, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.NotBeInRange))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortNotBeInRangeValidator.New(PrincipalChain, min, max))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty.
    /// </remarks>
    public ShortOperationsManager BeOneOf(Reason? reason = null, params short[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            string.Join(", ", expected.Select(BaseFormatter.Format)),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expected.IsNullOrEmpty(),
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
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="expected"/> is null or empty.
    /// </remarks>
    public ShortOperationsManager NotBeOneOf(Reason? reason = null, params short[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortNotBeOneOfValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            string.Join(", ", expected.Select(BaseFormatter.Format)),
                            BaseFormatter.Format(PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expected.IsNullOrEmpty(),
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
    /// <param name="divisor">The divisor to check divisibility against.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Fails immediately if <paramref name="divisor"/> is zero.
    /// </remarks>
    public ShortOperationsManager BeDivisibleBy(short divisor, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeDivisibleBy))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeDivisibleByValidator.New(PrincipalChain, divisor))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ShortOperationsManager BeEven(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeEven))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeEvenValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public ShortOperationsManager BeOdd(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Short.BeOdd))
        {
            return this;
        }

        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ShortBeOddValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
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
    public ShortOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
        {
            return this;
        }

        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public ShortOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
        {
            return this;
        }

        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public ShortOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
        {
            return this;
        }

        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public ShortOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
        {
            return this;
        }

        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<short>.New(PrincipalChain, type!))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        type is null,
                        Fail.New(
                            $"The {nameof(BeOfType)} operation failed because the expected type was <null>."
                        )
                    )
            )
            .Execute();
    }

    private void ValidateNotBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<ShortOperationsManager, short>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<short>.New(PrincipalChain, type!))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        type is null,
                        Fail.New(
                            $"The {nameof(NotBeOfType)} operation failed because the expected type was <null>."
                        )
                    )
            )
            .Execute();
    }
}
