using community.common.Extensions;
using SimpleInjector;

namespace community.ioc.Installers;

/// <summary>
///     Auto-registration of all provider classes inheriting from IProvider.
/// </summary>
public static class TypesInstaller
{
    /// <summary>
    ///     Registers the providers and their interfaces as scoped lifestyle.
    /// </summary>
    /// <param name="container"></param>
    public static void Register<T>(Container container)
    {
        var assemblies = AssemblyExtensions.FindAssemblies<T>();
        foreach (var assembly in assemblies)
            BaseInstaller<T>.RegisterTypes(container, assembly);
    }
}