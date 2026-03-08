using System.Reflection;
using JAAvila.FluentOperations.Common;
using JAAvila.SafeTypes.Extension;
using JAAvila.SafeTypes.Model;

namespace JAAvila.FluentOperations.Handler;

/// <summary>
/// Detects which test framework assembly is loaded in the current <see cref="AppDomain"/> by
/// inspecting the <see cref="TestFramework"/> enum metadata, and exposes the corresponding
/// assembly so that <see cref="ExceptionHandler"/> can instantiate the correct assertion exception.
/// Falls back to loading forced assemblies when automatic detection fails.
/// </summary>
internal class TestFrameworkHandler
{
    private static readonly Type Frameworks = typeof(TestFramework);
    public static List<EnumKeyValueData<int, string>> TestFrameworkAssemblyNames { get; } =
        Frameworks.GetKeyValues<int, string>("assembly");

    public static List<EnumKeyValueData<int, string>> TestFrameworkExceptionNamespace { get; } =
        Frameworks.GetKeyValues<int, string>("exception");

    private List<EnumBooleanData<int>> TestFrameworkForceAssembly { get; } =
        Frameworks.GetBooleanValues<int>();

    private Assembly? Framework => GetFrameworkAssemby();

    public Assembly? GetFramework() => Framework;

    #region PRIVATE METHODS

    private Assembly? GetFrameworkAssemby()
    {
        var assemblyNames = TestFrameworkAssemblyNames.Select(x => x.ObjectValue).ToArray();
        var assembly = AppDomain
            .CurrentDomain.GetAssemblies()
            .FirstOrDefault(x => assemblyNames.Contains(x.GetName().Name.SafeNull().ToLower()));

        if (assembly is not null)
        {
            return assembly;
        }

        var fa = TestFrameworkForceAssembly.Where(x => x.BooleanValue);

        foreach (var a in fa)
        {
            try
            {
                var ns = TestFrameworkAssemblyNames.FirstOrDefault(x => x.Value == a.Value);

                if (ns is null)
                {
                    continue;
                }

                assembly = Assembly.Load(new AssemblyName(ns.ObjectValue));

                break;
            }
            catch
            {
                // ignored
            }
        }

        return assembly;
    }

    #endregion
}
