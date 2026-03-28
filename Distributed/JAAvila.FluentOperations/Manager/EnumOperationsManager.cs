using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for enum values.
/// Supports equality, flag, definition, range, and membership validations.
/// </summary>
public class EnumOperationsManager<T> : ITestManager<EnumOperationsManager<T>, T>
    where T : Enum
{
    /// <inheritdoc />
    public PrincipalChain<T> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public EnumOperationsManager(T value, string caller)
    {
        PrincipalChain = PrincipalChain<T>.Get(value, caller);
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
    /// DayOfWeek.Monday.TestEnum().Be(DayOfWeek.Monday);
    /// </code>
    /// </example>
    public EnumOperationsManager<T> Be(T expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.Be))
        {
            return this;
        }

        ExecutionEngine<EnumOperationsManager<T>, T>
            .New(this)
            .WithOperation(EnumBeValidator<T>.New(PrincipalChain, expected))
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
    public EnumOperationsManager<T> NotBe(T expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.NotBe))
        {
            return this;
        }

        ExecutionEngine<EnumOperationsManager<T>, T>
            .New(this)
            .WithOperation(EnumNotBeValidator<T>.New(PrincipalChain, expected))
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
    /// Asserts that the value is a defined member of the <typeparamref name="T"/> enum type.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public EnumOperationsManager<T> BeDefined(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.BeDefined))
        {
            return this;
        }

        ExecutionEngine<EnumOperationsManager<T>, T>
            .New(this)
            .WithOperation(EnumBeDefinedValidator<T>.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue()),
                            typeof(T).Name
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is one of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public EnumOperationsManager<T> BeOneOf(params T[] expected)
    {
        return BeOneOf(null, expected);
    }

    /// <summary>
    /// Asserts that the value is one of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expected"/> is empty (empty set guard).
    /// </remarks>
    public EnumOperationsManager<T> BeOneOf(Reason? reason, params T[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<EnumOperationsManager<T>, T>
            .New(this)
            .WithOperation(EnumBeOneOfValidator<T>.New(PrincipalChain, expected))
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
                        expected.Length == 0,
                        Fail.New(
                            $"The {nameof(BeOneOf)} operation failed because you have not provided any values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is not any of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public EnumOperationsManager<T> NotBeOneOf(params T[] expected)
    {
        return NotBeOneOf(null, expected);
    }

    /// <summary>
    /// Asserts that the value is not any of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expected"/> is empty (empty set guard).
    /// </remarks>
    public EnumOperationsManager<T> NotBeOneOf(Reason? reason, params T[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<EnumOperationsManager<T>, T>
            .New(this)
            .WithOperation(EnumNotBeOneOfValidator<T>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            string.Join(", ", expected.Select(BaseFormatter.Format))
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expected.Length == 0,
                        Fail.New(
                            $"The {nameof(NotBeOneOf)} operation failed because you have not provided any values."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value has the specified <paramref name="flag"/> set
    /// (for <c>[Flags]</c> enums).
    /// </summary>
    /// <param name="flag">The flag to check for.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public EnumOperationsManager<T> HaveFlag(T flag, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.HaveFlag))
        {
            return this;
        }

        ExecutionEngine<EnumOperationsManager<T>, T>
            .New(this)
            .WithOperation(EnumHaveFlagValidator<T>.New(PrincipalChain, flag))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue()),
                            BaseFormatter.Format(flag)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value does not have the specified <paramref name="flag"/> set
    /// (for <c>[Flags]</c> enums).
    /// </summary>
    /// <param name="flag">The flag that must not be set.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public EnumOperationsManager<T> NotHaveFlag(T flag, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.NotHaveFlag))
        {
            return this;
        }

        ExecutionEngine<EnumOperationsManager<T>, T>
            .New(this)
            .WithOperation(EnumNotHaveFlagValidator<T>.New(PrincipalChain, flag))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue()),
                            BaseFormatter.Format(flag)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public EnumOperationsManager<T> BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public EnumOperationsManager<T> BeOfType(Type expected, Reason? reason = null)
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
    public EnumOperationsManager<T> NotBeOfType<TType>(Reason? reason = null)
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
    public EnumOperationsManager<T> NotBeOfType(Type expected, Reason? reason = null)
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
        ExecutionEngine<EnumOperationsManager<T>, T>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<T>.New(PrincipalChain, type!))
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
        ExecutionEngine<EnumOperationsManager<T>, T>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<T>.New(PrincipalChain, type!))
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
