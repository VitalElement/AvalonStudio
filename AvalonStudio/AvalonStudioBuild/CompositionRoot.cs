using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Utils;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace AvalonStudio
{
    /*internal static class CompositionRoot
    {
        public static CompositionHost CreateContainer(ExtensionManager extensionManager)
        {
            var conventions = new ConventionBuilder();
            conventions.ForTypesDerivedFrom<IExtension>().Export<IExtension>();

            // TODO AppDomain here is a custom appdomain from namespace AvalonStudio.Extensibility.Utils. It is able
            // to load any assembly in the bin directory (so not really appdomain) we need to get rid of this
            // once all our default extensions are published with a manifest and copied to extensions dir.
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var extensionAssemblies = LoadMefComponents(extensionManager);

            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies, conventions)
                .WithAssemblies(extensionAssemblies);
            return configuration.CreateContainer();
        }

        private static IEnumerable<Assembly> LoadMefComponents(ExtensionManager extensionManager)
        {
            var assemblies = new List<Assembly>();

            foreach (var extension in extensionManager.GetInstalledExtensions())
            {
                foreach (var mefComponent in extension.GetMefComponents())
                {
                    try
                    {
                        assemblies.Add(Assembly.LoadFrom(mefComponent));
                    }
                    catch (System.Exception e)
                    {
                        System.Console.WriteLine($"Failed to load MEF component from extension: '{mefComponent}'");
                        System.Console.WriteLine(e.ToString());
                    }
                }
            }

            return assemblies;
        }
    }*/
}