using System.Reflection;

namespace community.common.Extensions;

/// <summary>
///     Methods related to working with the Assembly.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    ///     Finds all assemblies that implement type T.
    /// </summary>
    /// <typeparam name="T">The empty interface that is being used to register tyeps.</typeparam>
    public static IEnumerable<Assembly> FindAssemblies<T>()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => assembly.GetExportedTypes().Any(t =>
                t.IsClass && typeof(T).IsAssignableFrom(t) && !t.IsAbstract)).ToList();
    }

    /// <summary>
    ///     Recursively loads the referenced ZOLL assemblies and their internal dependencies.
    /// </summary>
    /// <param name="assembly">The assembly to scan for references to load.</param>
    private static void LoadReferencedAssembly(Assembly assembly)
    {
        foreach (var name in assembly
                     .GetReferencedAssemblies()
                     .Where(a => a.FullName.ToLower()
                         .StartsWith("community", StringComparison.InvariantCultureIgnoreCase)))
            if (AppDomain.CurrentDomain.GetAssemblies().All(a => a.FullName != name.FullName))
                LoadReferencedAssembly(Assembly.Load(name));
    }
}