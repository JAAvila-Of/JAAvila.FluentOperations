using System.Runtime.CompilerServices;

// Allow the Architecture package to access internal types (e.g., ReflectionDependencyScanner)
// needed to implement the Cecil scanner that extends the Core scanner.
[assembly: InternalsVisibleTo("JAAvila.FluentOperations.Architecture")]
