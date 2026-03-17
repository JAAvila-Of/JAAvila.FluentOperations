using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>T?</c> nullable enum values.
/// Supports value presence, null-checking, equality, flag, definition, and membership validations.
/// </summary>
public class NullableEnumOperationsManager<T>
    : BaseNullableOperationsManager<NullableEnumOperationsManager<T>, T?>,
        ITestManager<NullableEnumOperationsManager<T>, T?>
    where T : struct, Enum
{
    /// <inheritdoc />
    public PrincipalChain<T?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified nullable enum value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableEnumOperationsManager(T? value, string caller)
    {
        PrincipalChain = PrincipalChain<T?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable enum has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableEnumOperationsManager<T> HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableEnum.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(NullableEnumHaveValueValidator<T>.New(PrincipalChain))
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
    /// Asserts that the nullable enum is <c>null</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableEnumOperationsManager<T> NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableEnum.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(NullableEnumNotHaveValueValidator<T>.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            PrincipalChain.GetValue()?.ToString() ?? "<null>"
                        )
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
    public NullableEnumOperationsManager<T> Be(T? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.Be))
        {
            return this;
        }

        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(NullableEnumBeValidator<T>.New(PrincipalChain, expected))
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
    public NullableEnumOperationsManager<T> NotBe(T? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(NullableEnumNotBeValidator<T>.New(PrincipalChain, expected))
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
    /// Asserts that the value is a defined member of the <typeparamref name="T"/> enum type.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableEnumOperationsManager<T> BeDefined(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.BeDefined))
        {
            return this;
        }

        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(NullableEnumBeDefinedValidator<T>.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue()),
                            typeof(T).Name
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeDefined)} operation failed because the value was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is one of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableEnumOperationsManager<T> BeOneOf(params T[] expected)
    {
        return BeOneOf(null, expected);
    }

    /// <summary>
    /// Asserts that the value is one of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of allowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableEnumOperationsManager<T> BeOneOf(Reason? reason, params T[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.BeOneOf))
        {
            return this;
        }

        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(NullableEnumBeOneOfValidator<T>.New(PrincipalChain, expected))
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
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeOneOf)} operation failed because the value was null."
                        )
                    )
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
    public NullableEnumOperationsManager<T> NotBeOneOf(params T[] expected)
    {
        return NotBeOneOf(null, expected);
    }

    /// <summary>
    /// Asserts that the value is not any of the specified <paramref name="expected"/> values.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <param name="expected">The set of disallowed values.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableEnumOperationsManager<T> NotBeOneOf(Reason? reason, params T[] expected)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.NotBeOneOf))
        {
            return this;
        }

        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(NullableEnumNotBeOneOfValidator<T>.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            string.Join(", ", expected.Select(e => BaseFormatter.Format(e)))
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeOneOf)} operation failed because the value was null."
                        )
                    )
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
    public NullableEnumOperationsManager<T> HaveFlag(T flag, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.HaveFlag))
        {
            return this;
        }

        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(NullableEnumHaveFlagValidator<T>.New(PrincipalChain, flag))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue()),
                            BaseFormatter.Format(flag)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(HaveFlag)} operation failed because the value was null."
                        )
                    )
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
    public NullableEnumOperationsManager<T> NotHaveFlag(T flag, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Enum.NotHaveFlag))
        {
            return this;
        }

        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(NullableEnumNotHaveFlagValidator<T>.New(PrincipalChain, flag))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.ResultValidation,
                            BaseFormatter.Format(PrincipalChain.GetValue()),
                            BaseFormatter.Format(flag)
                        )
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotHaveFlag)} operation failed because the value was null."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public NullableEnumOperationsManager<T> BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public NullableEnumOperationsManager<T> BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public NullableEnumOperationsManager<T> NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public NullableEnumOperationsManager<T> NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<T?>.New(PrincipalChain, type!))
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
        ExecutionEngine<NullableEnumOperationsManager<T>, T?>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<T?>.New(PrincipalChain, type!))
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