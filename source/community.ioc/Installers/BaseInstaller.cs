using System.Reflection;
using community.common.Interfaces;
using SimpleInjector;

namespace community.ioc.Installers;

/// <summary>
///     Abstract auto-installer for registering interfaces and their implementations.
/// </summary>
/// <typeparam name="T">The type param indicating what is being regisered, repository classes, context or providers.</typeparam>
public static class BaseInstaller<T>
{
    /// <summary>
    ///     Auto-registers repositories that inherit from the empty <see cref="IRepository" />,
    ///     <see cref="IProvider" />, or <see cref="IDapperContext" /> interface defined in the Common library.
    /// </summary>
    /// <param name="container">The SimpleInjector container where the services are being registered.</param>
    /// <param name="assembly">The assembly to scan for specified types.</param>
    public static void RegisterTypes(Container container, Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.IsClass && typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();

        types.ForEach(ctx =>
        {
            foreach (var interf in ctx.GetInterfaces().Where(i => i != typeof(T)))
                container.Register(interf, ctx, Lifestyle.Scoped);
        });
    }
}