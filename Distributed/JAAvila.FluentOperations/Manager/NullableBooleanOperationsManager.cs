using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides fluent validation operations for <c>bool?</c> values.
/// Supports equality, value presence, collective truth, and logical implication validations.
/// </summary>
public class NullableBooleanOperationsManager
    : BaseNullableOperationsManager<NullableBooleanOperationsManager, bool?>,
        ITestManager<NullableBooleanOperationsManager, bool?>
{
    /// <inheritdoc />
    public PrincipalChain<bool?> PrincipalChain { get; }

    /// <summary>
    /// Initializes a new instance for testing the specified nullable value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="caller">The caller expression name, captured automatically.</param>
    public NullableBooleanOperationsManager(bool? value, string caller)
    {
        PrincipalChain = PrincipalChain<bool?>.Get(value, caller);
        GlobalConfig.Initialize();
        SetManager(this);
    }

    /// <summary>
    /// Asserts that the nullable value has a non-null value.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableBooleanOperationsManager HaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableBoolean.HaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableBooleanOperationsManager, bool?>
            .New(this)
            .WithOperation(NullableBooleanHaveValueValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the nullable value is <c>null</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableBooleanOperationsManager NotHaveValue(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.NullableBoolean.NotHaveValue))
        {
            return this;
        }

        ExecutionEngine<NullableBooleanOperationsManager, bool?>
            .New(this)
            .WithOperation(NullableBooleanNotHaveValueValidator.New(PrincipalChain))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
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
    public NullableBooleanOperationsManager Be(bool? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Boolean.Be))
        {
            return this;
        }

        ExecutionEngine<NullableBooleanOperationsManager, bool?>
            .New(this)
            .WithOperation(NullableBooleanBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey, operation.ResultValidation,
                            BooleanFormatter.Format(PrincipalChain.GetValue()),
                            BooleanFormatter.Format(expected)
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
    public NullableBooleanOperationsManager NotBe(bool? expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Boolean.NotBe))
        {
            return this;
        }

        ExecutionEngine<NullableBooleanOperationsManager, bool?>
            .New(this)
            .WithOperation(NullableBooleanNotBeValidator.New(PrincipalChain, expected))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation, BooleanFormatter.Format(expected))
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the value is <c>true</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableBooleanOperationsManager BeTrue(Reason? reason = null)
    {
        return Be(true, reason);
    }

    /// <summary>
    /// Asserts that the value is <c>false</c>.
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableBooleanOperationsManager BeFalse(Reason? reason = null)
    {
        return Be(false, reason);
    }

    /// <summary>
    /// Asserts that the value is not <c>true</c> (i.e., is <c>false</c> or <c>null</c>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableBooleanOperationsManager NotBeTrue(Reason? reason = null)
    {
        return NotBe(true, reason);
    }

    /// <summary>
    /// Asserts that the value is not <c>false</c> (i.e., is <c>true</c> or <c>null</c>).
    /// </summary>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public NullableBooleanOperationsManager NotBeFalse(Reason? reason = null)
    {
        return NotBe(false, reason);
    }

    /// <summary>
    /// Asserts that all of the provided <paramref name="booleans"/> are <c>true</c>.
    /// </summary>
    /// <param name="booleans">The boolean values to check.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the subject is <c>null</c>, no arguments are provided, or any argument is <c>null</c>.
    /// </remarks>
    public NullableBooleanOperationsManager BeAllTrue(params bool?[] booleans)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Boolean.BeAllTrue))
        {
            return this;
        }

        ExecutionEngine<NullableBooleanOperationsManager, bool?>
            .New(this)
            .WithOperation(NullableBooleanBeAllTrueValidator.New(PrincipalChain, booleans))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeAllTrue)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        booleans.Length == 0,
                        Fail.New(
                            $"The {nameof(BeAllTrue)} operation failed because you have not provided any boolean arguments."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        booleans.Any(x => x is null),
                        Fail.New(
                            $"The {nameof(BeAllTrue)} operation failed because one of the boolean arguments has a <null> value."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that all of the provided <paramref name="booleans"/> are <c>false</c>.
    /// </summary>
    /// <param name="booleans">The boolean values to check.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the subject is <c>null</c>, no arguments are provided, or any argument is <c>null</c>.
    /// </remarks>
    public NullableBooleanOperationsManager BeAllFalse(params bool?[] booleans)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Boolean.BeAllFalse))
        {
            return this;
        }

        ExecutionEngine<NullableBooleanOperationsManager, bool?>
            .New(this)
            .WithOperation(NullableBooleanBeAllFalseValidator.New(PrincipalChain, booleans))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
            )
            .FailIf(
                manager =>
                    (
                        manager.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeAllFalse)} operation failed because the resulting value was <null>, use {nameof(NotBeNull)} to cover all possible scenarios."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        booleans.Length == 0,
                        Fail.New(
                            $"The {nameof(BeAllFalse)} operation failed because you have not provided any boolean arguments."
                        )
                    )
            )
            .FailIf(
                _ =>
                    (
                        booleans.Any(x => x is null),
                        Fail.New(
                            $"The {nameof(BeAllFalse)} operation failed because one of the boolean arguments has a <null> value."
                        )
                    )
            )
            .Execute();

        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="TType"/>.
    /// </summary>
    public NullableBooleanOperationsManager BeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    public NullableBooleanOperationsManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
            return this;
        ValidateBeOfTypeOperation(reason, expected);
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="TType"/>.
    /// </summary>
    public NullableBooleanOperationsManager NotBeOfType<TType>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, typeof(TType));
        return this;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    public NullableBooleanOperationsManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
            return this;
        ValidateNotBeOfTypeOperation(reason, expected);
        return this;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<NullableBooleanOperationsManager, bool?>
            .New(this)
            .WithOperation(ReferenceBeOfTypeValidator<bool?>.New(PrincipalChain, type!))
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
        ExecutionEngine<NullableBooleanOperationsManager, bool?>
            .New(this)
            .WithOperation(ReferenceNotBeOfTypeValidator<bool?>.New(PrincipalChain, type!))
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
