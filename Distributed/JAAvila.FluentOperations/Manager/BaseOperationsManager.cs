using System.Linq.Expressions;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Utils;
using JAAvila.FluentOperations.Validators;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Manager;

/// <summary>
/// Provides common operations (type checking, reference equality, casting, evaluate) for all non-nullable typed managers.
/// </summary>
public abstract class BaseOperationsManager<TManager, TSubject>
    : BaseNullableOperationsManager<TManager, TSubject>
    where TManager : ITestManager<TManager, TSubject>
{
    private TManager Manager { get; set; } = default!;

    /// <summary>
    /// Sets the concrete manager instance used for method chaining returns.
    /// Must be called in the constructor of each derived class.
    /// </summary>
    /// <param name="manager">The concrete manager instance.</param>
    protected override void SetManager(TManager manager)
    {
        Manager = manager;
        base.SetManager(manager);
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The expected runtime type.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or if the expected type is <c>null</c>.
    /// </remarks>
    public TManager BeOfType<T>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
        {
            return Manager;
        }

        var type = typeof(T);

        ValidateBeOfTypeOperation(reason, type);

        return Manager;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is exactly <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected runtime type.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or if <paramref name="expected"/> is <c>null</c>.
    /// </remarks>
    public TManager BeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeOfType))
        {
            return Manager;
        }

        ValidateBeOfTypeOperation(reason, expected);

        return Manager;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type that should not match.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or if the expected type is <c>null</c>.
    /// </remarks>
    public TManager NotBeOfType<T>(Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
        {
            return Manager;
        }

        var type = typeof(T);

        ValidateNotBeOfTypeOperation(reason, type);

        return Manager;
    }

    /// <summary>
    /// Asserts that the runtime type of the value is not <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The type that should not match.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or if <paramref name="expected"/> is <c>null</c>.
    /// </remarks>
    public TManager NotBeOfType(Type expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeOfType))
        {
            return Manager;
        }

        ValidateNotBeOfTypeOperation(reason, expected);

        return Manager;
    }

    /// <summary>
    /// Asserts that the value refers to the same object instance as <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected object reference.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TManager BeSameAs(TSubject expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.BeSameAs))
        {
            return Manager;
        }

        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(
                ReferenceBeSameAsValidator<TSubject>.New(Manager.PrincipalChain, expected)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(expected),
                            BaseFormatter.Format(Manager.PrincipalChain.GetValue())
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return Manager;
    }

    /// <summary>
    /// Asserts that the value does not refer to the same object instance as <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The object reference that should not be the same.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    public TManager NotBeSameAs(TSubject expected, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.NotBeSameAs))
        {
            return Manager;
        }

        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(
                ReferenceNotBeSameAsValidator<TSubject>.New(Manager.PrincipalChain, expected)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
                        .WithResult(
                            operation.MessageKey,
                            operation.ResultValidation,
                            BaseFormatter.Format(expected)
                        )
                        .WithReason(reason?.ToString())
            )
            .Execute();

        return Manager;
    }

    /// <summary>
    /// Asserts that the value can be cast to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The target type for the cast.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c>.
    /// </remarks>
    public TManager BeCastTo<T>(Reason? reason = null)
    {
        return !OperationUtils.CheckOperationAllowed(Operations.Common.BeCastTo)
            ? Manager
            : ValidateBeCastToByGenericOperation<T>(reason);
    }

    /// <summary>
    /// Asserts that the value can be cast to <paramref name="expectedType"/>.
    /// </summary>
    /// <param name="expectedType">The target type for the cast.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or if <paramref name="expectedType"/> is <c>null</c>.
    /// </remarks>
    public TManager BeCastTo(Type expectedType, Reason? reason = null)
    {
        return !OperationUtils.CheckOperationAllowed(Operations.Common.BeCastTo)
            ? Manager
            : ValidateBeCastToOperation(reason, expectedType);
    }

    /// <summary>
    /// Asserts that the value cannot be cast to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type the value should not be castable to.</typeparam>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c>.
    /// </remarks>
    public TManager NotBeCastTo<T>(Reason? reason = null)
    {
        return !OperationUtils.CheckOperationAllowed(Operations.Common.NotBeCastTo)
            ? Manager
            : ValidateNotBeCastToOperation(reason, typeof(T));
    }

    /// <summary>
    /// Asserts that the value cannot be cast to <paramref name="expectedType"/>.
    /// </summary>
    /// <param name="expectedType">The type the value should not be castable to.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if the value is <c>null</c> or if <paramref name="expectedType"/> is <c>null</c>.
    /// </remarks>
    public TManager NotBeCastTo(Type expectedType, Reason? reason = null)
    {
        return !OperationUtils.CheckOperationAllowed(Operations.Common.NotBeCastTo)
            ? Manager
            : ValidateNotBeCastToOperation(reason, expectedType);
    }

    /// <summary>
    /// Asserts that the value satisfies the given boolean <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">A lambda expression that must evaluate to <c>true</c>.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="expression"/> is <c>null</c>.
    /// </remarks>
    public TManager Evaluate(Expression<Func<TSubject, bool>> expression, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.Evaluate))
        {
            return Manager;
        }

        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(
                ReferenceEvaluateExpressionValidator<TSubject>.New(
                    Manager.PrincipalChain,
                    expression
                )
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithExpression(expression.ToString())
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expression.IsNull(),
                        Fail.New(
                            $"The {nameof(Evaluate)} operation failed because the expected expression was <null>."
                        )
                    )
            )
            .Execute();

        return Manager;
    }

    /// <summary>
    /// Runs the given <paramref name="action"/> against the value cast as <typeparamref name="TType"/>,
    /// collecting all inner assertion failures into a single transactional failure report.
    /// </summary>
    /// <typeparam name="TType">The type to cast the value to before passing it to the action.</typeparam>
    /// <param name="action">The inspection action to run against the cast value.</param>
    /// <param name="mode">The transactional mode that controls how failures are accumulated and surfaced.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="action"/> is <c>null</c>, the value is <c>null</c>,
    /// or the value cannot be cast to <typeparamref name="TType"/>.
    /// </remarks>
    public TManager Evaluate<TType>(
        Action<TType> action,
        TransactionalMode mode = TransactionalMode.AccumulateFailsAndDisposeThis
    )
        where TType : TSubject
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.Evaluate))
        {
            return Manager;
        }

        using var transaction = new TransactionalOperations(mode);

        transaction.SetHeader(
            Template.New(
                "The evaluation of {0} of type {1} has not been satisfactory and the following observations have been found:",
                Manager.PrincipalChain.GetSubject(),
                TypeFormatter.FriendlyName(typeof(TType))
            )
        );

        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(
                ReferenceEvaluateActionValidator<TSubject, TType>.New(
                    Manager.PrincipalChain,
                    action
                )
            )
            .FailIf(
                _ =>
                    (
                        action.IsNull(),
                        Fail.New(
                            $"The {nameof(Evaluate)} operation failed because the inspector action was <null>."
                        )
                    )
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(Evaluate)} operation failed because the resulting value was <null>."
                        )
                    )
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is not TType,
                        Fail.New(
                            $"The {nameof(Evaluate)} operation failed because the resulting value should be assignable to {{0}}, but {{1}} was found.",
                            TypeFormatter.FriendlyName(typeof(TType)),
                            TypeFormatter.FriendlyName(m.PrincipalChain.GetValue()!.GetType())
                        )
                    )
            )
            .Execute();

        return Manager;
    }

    /// <summary>
    /// Asserts that the value passes the given <see cref="ICustomValidator{TSubject}"/>.
    /// </summary>
    /// <param name="customValidator">The custom validator to evaluate.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>The current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="customValidator"/> is <c>null</c>.
    /// </remarks>
    public TManager Evaluate(ICustomValidator<TSubject> customValidator, Reason? reason = null)
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.Evaluate))
        {
            return Manager;
        }

        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(
                ReferenceEvaluateCustomValidator<TSubject>.New(
                    Manager.PrincipalChain,
                    customValidator
                )
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        customValidator.IsNull(),
                        Fail.New(
                            $"The {nameof(Evaluate)} operation failed because the custom validator was <null>."
                        )
                    )
            )
            .Execute();

        return Manager;
    }

    /// <summary>
    /// Asserts asynchronously that the value passes the given <see cref="IAsyncCustomValidator{TSubject}"/>.
    /// </summary>
    /// <param name="customValidator">The async custom validator to evaluate.</param>
    /// <param name="reason">An optional reason providing context for the assertion.</param>
    /// <returns>A task that completes with the current manager instance for method chaining.</returns>
    /// <remarks>
    /// Throws immediately if <paramref name="customValidator"/> is <c>null</c>.
    /// </remarks>
    public async Task<TManager> EvaluateAsync(
        IAsyncCustomValidator<TSubject> customValidator,
        Reason? reason = null
    )
    {
        if (!OperationUtils.CheckOperationAllowed(Operations.Common.Evaluate))
        {
            return Manager;
        }

        await ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(
                ReferenceEvaluateAsyncCustomValidator<TSubject>.New(
                    Manager.PrincipalChain,
                    customValidator
                )
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        customValidator.IsNull(),
                        Fail.New(
                            $"The {nameof(EvaluateAsync)} operation failed because the async custom validator was <null>."
                        )
                    )
            )
            .ExecuteAsync();

        return Manager;
    }

    #region PRIVATE METHODS

    private TManager ValidateNotBeCastToOperation(Reason? reason, Type? expectedType)
    {
        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(
                ReferenceNotBeCastToValidator<TSubject>.New(Manager.PrincipalChain, expectedType!)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expectedType is null,
                        Fail.New(
                            $"The {nameof(NotBeCastTo)} operation failed because the expected type was <null>."
                        )
                    )
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeCastTo)} operation failed because the parent value was <null>, {{0}}.",
                            $"it is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return Manager;
    }

    private TManager ValidateBeCastToByGenericOperation<TType>(Reason? reason)
    {
        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(
                ReferenceBeCastToByGenericValidator<TSubject, TType>.New(Manager.PrincipalChain)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeCastTo)} operation failed because the parent value was <null>, {{0}}.",
                            $"it is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return Manager;
    }

    private TManager ValidateBeCastToOperation(Reason? reason, Type? expectedType)
    {
        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(
                ReferenceBeCastToValidator<TSubject>.New(Manager.PrincipalChain, expectedType!)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
                        .WithResult(operation.MessageKey, operation.ResultValidation)
                        .WithReason(reason?.ToString())
            )
            .FailIf(
                _ =>
                    (
                        expectedType is null,
                        Fail.New(
                            $"The {nameof(BeCastTo)} operation failed because the expected type was <null>."
                        )
                    )
            )
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeCastTo)} operation failed because the parent value was <null>, {{0}}.",
                            $"it is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();

        return Manager;
    }

    private void ValidateBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(ReferenceBeOfTypeValidator<TSubject>.New(Manager.PrincipalChain, type!))
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
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
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(BeOfType)} operation failed because the parent value was <null>, {{0}}.",
                            $"it is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();
    }

    private void ValidateNotBeOfTypeOperation(Reason? reason, Type? type)
    {
        ExecutionEngine<TManager, TSubject>
            .New(Manager)
            .WithOperation(
                ReferenceNotBeOfTypeValidator<TSubject>.New(Manager.PrincipalChain, type!)
            )
            .WithTemplate(
                (template, operation) =>
                    template
                        .WithSubject(Manager.PrincipalChain.GetSubject())
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
            .FailIf(
                m =>
                    (
                        m.PrincipalChain.GetValue() is null,
                        Fail.New(
                            $"The {nameof(NotBeOfType)} operation failed because the parent value was <null>, {{0}}.",
                            $"it is recommended to run the {nameof(NotBeNull)} operation first to cover all possible scenarios"
                        )
                    )
            )
            .Execute();
    }

    #endregion
}
