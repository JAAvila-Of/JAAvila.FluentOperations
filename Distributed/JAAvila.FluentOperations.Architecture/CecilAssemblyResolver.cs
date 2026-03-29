using Mono.Cecil;

namespace JAAvila.FluentOperations.Architecture;

/// <summary>
/// Custom assembly resolver for Mono.Cecil that adds the scanned assembly's
/// directory and the .NET runtime directory to the search paths.
/// </summary>
internal sealed class CecilAssemblyResolver : DefaultAssemblyResolver
{
    /// <param name="assemblyDirectory">
    /// The directory containing the assembly being analyzed.
    /// </param>
    public CecilAssemblyResolver(string assemblyDirectory)
    {
        AddSearchDirectory(assemblyDirectory);

        // Add .NET runtime directory for BCL assembly resolution
        var runtimeDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();

        if (!string.IsNullOrEmpty(runtimeDir))
        {
            AddSearchDirectory(runtimeDir);
        }
    }
}
