using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Extensibility.Utils;
using AvalonStudio.Projects;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;

namespace AvalonStudio
{
    internal static class CompositionRoot
    {
        private static readonly string PluginsFolder = "Plugins";

        public static CompositionHost CreateContainer()
        {
            EnsurePluginsFolder();

            var conventions = new ConventionBuilder();

            conventions.ForTypesDerivedFrom<IExtension>().Export<IExtension>();            

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var configuration = new ContainerConfiguration().WithAssemblies(assemblies, conventions);

            return configuration.CreateContainer();
        }

        private static void EnsurePluginsFolder()
        {
            Directory.CreateDirectory(PluginsFolder);
        }
    }
}