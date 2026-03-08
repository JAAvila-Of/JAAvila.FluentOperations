using System.Collections.Concurrent;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

/// <summary>
/// Carries the typed value under test and its subject name (caller expression or property name)
/// through the validation chain. Instances are keyed by a <see cref="Guid"/> chain ID and stored
/// in an <see cref="AsyncLocal{T}"/>-backed stack so that nested or concurrent chains remain
/// isolated. Each manager creates one <c>PrincipalChain</c> and passes it to its validators.
/// </summary>
/// <typeparam name="T">The type of value being validated.</typeparam>
public class PrincipalChain<T> : IDisposable
{
    private static readonly AsyncLocal<ConcurrentDictionary<Guid, Stack<BaseChain<T>>>> Contexts =
        new();

    private static readonly AsyncLocal<PrincipalChain<T>?> Instance = new();

    private readonly Guid _chainId;

    private PrincipalChain(Guid chainId, BaseChain<T> value)
    {
        _chainId = chainId;
        Contexts.Value ??= new ConcurrentDictionary<Guid, Stack<BaseChain<T>>>();

        var stack = new Stack<BaseChain<T>>();
        stack.Push(value);

        Contexts.Value.TryAdd(chainId, stack);
    }

    /// <summary>
    /// Returns the active <see cref="PrincipalChain{T}"/> for the current async context when
    /// continuation mode is set, or creates a new chain with a fresh <see cref="Guid"/> identifier.
    /// Used by manager constructors as the single entry point for chain creation.
    /// </summary>
    /// <param name="value">The initial value to place on the chain.</param>
    /// <param name="caller">The caller expression or property name that identifies the subject.</param>
    /// <returns>A <see cref="PrincipalChain{T}"/> ready for use in a manager.</returns>
    public static PrincipalChain<T> Get(T value, string caller)
    {
        if (Instance.Value is not null)
        {
            var instance = Instance.Value;

            instance.AddStack(
                BaseChain<T>.Create(BaseValue<T>.Create(value), BaseSubject.Create(caller))
            );

            Instance.Value = null;

            return instance;
        }

        var chainId = Guid.NewGuid();

        return new PrincipalChain<T>(
            chainId,
            BaseChain<T>.Create(BaseValue<T>.Create(value), BaseSubject.Create(caller))
        );
    }

    /// <summary>
    /// Replaces the current value on the top of the chain stack.
    /// Called by <c>IQualityRule.SetValue</c> during Blueprint execution to inject the real model
    /// property value before a captured validator runs.
    /// </summary>
    /// <param name="value">The new value to set.</param>
    public void SetValue(T value)
    {
        GetStack().Peek().BaseValue.Value = value;
    }

    /// <summary>
    /// Reads the current value from the top of the chain stack.
    /// Validators call this to obtain the value they should validate.
    /// </summary>
    /// <returns>The current value under test.</returns>
    public T GetValue()
    {
        return GetStack().Peek().BaseValue.Value;
    }

    internal BaseChain<T> GetBaseChain()
    {
        return GetStack().Peek();
    }

    /// <summary>
    /// Returns the subject label (caller expression or property name) associated with the current
    /// chain entry. Used by <see cref="Handler.TemplateHandler"/> to build failure messages.
    /// </summary>
    /// <returns>A string identifying the subject being validated.</returns>
    public string GetSubject()
    {
        return GetStack().Peek().BaseSubject.ToString();
    }

    public string? GetValueAsString()
    {
        return GetValue()?.ToString();
    }

    internal SubjectType GetSubjectType()
    {
        return GetStack().Peek().BaseSubject.SubjectType.Value;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (!Contexts.Value!.TryGetValue(_chainId, out var stack) || stack.Count <= 0)
        {
            return;
        }

        stack.Pop();

        if (stack.Count == 0)
        {
            Contexts.Value.TryRemove(_chainId, out _);
        }
    }

    protected Guid GetCurrentChainId()
    {
        return _chainId;
    }

    internal void AddStack(BaseChain<T> value)
    {
        GetStack().Push(value);
    }

    protected void CleanInstance()
    {
        Instance.Value = null;
    }

    /// <summary>
    /// Marks this chain as the active continuation instance so the next call to
    /// <see cref="Get"/> within the same async context reuses it rather than creating a new one.
    /// Used to carry the same chain across multiple chained manager method calls.
    /// </summary>
    public void Continue()
    {
        Instance.Value = this;
    }

    #region PRIVATE METHODS

    private Stack<BaseChain<T>> GetStack()
    {
        return Contexts.Value![_chainId];
    }

    #endregion
}
