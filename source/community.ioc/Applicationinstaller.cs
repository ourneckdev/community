using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using community.common.Interfaces;
using community.ioc.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace community.ioc;

/// <summary>
///     Simple Injector Application Installer
/// </summary>
[ExcludeFromCodeCoverage]
public static class ApplicationInstaller
{
    /// <summary>
    ///     Sets up the DI container in SimpleInjector and passing the configuration back to the .Net container.
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static Container Install(IConfiguration configuration)
    {
        var container = new Container();
        container.Options.AllowOverridingRegistrations = true;
        container.Options.DefaultLifestyle = Lifestyle.Scoped;
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            LoadReferencedAssembly(assembly);

        TypesInstaller.Register<IProvider>(container);
        TypesInstaller.Register<IFactory>(container);
        TypesInstaller.Register<IRepository>(container);
        TypesInstaller.Register<IDapperContext>(container);

        container.RegisterSingleton(typeof(ILogger<>), typeof(Logger<>));
        return container;
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