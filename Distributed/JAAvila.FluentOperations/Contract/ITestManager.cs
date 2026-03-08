namespace JAAvila.FluentOperations.Contract;

/// <summary>
/// Represents an interface for managing tests with the associated principal chain and test list.
/// </summary>
public interface ITestManager<out TManager, TS> : ITestManager
    where TManager : ITestManager<TManager, TS>
{
    /// <summary>
    /// The principal chain that carries the subject value and its name through the assertion chain.
    /// </summary>
    PrincipalChain<TS> PrincipalChain { get; }
}

/// <summary>
/// Non-generic marker interface for all test managers. Allows untyped discovery and registration.
/// </summary>
public interface ITestManager;
