using System.Collections.Generic;
using System.Composition.Hosting;
using System.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using AvalonStudio.Languages;
using System.Composition.Convention;
using Microsoft.Extensions.DependencyModel;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Debugging;
using AvalonStudio.Extensibility.Utils;

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

        //private static List<ComposablePartCatalog> GetCatalogsToImport()
        //{
        //	var pluginsCatalog = new DirectoryCatalog(PluginsFolder);
        //	var assemblyCatalogs = ScannedAssemblies.Select(assembly => new AssemblyCatalog(assembly));

        //	var catalogs = new List<ComposablePartCatalog>();
        //	catalogs.Add(pluginsCatalog);
        //	catalogs.AddRange(assemblyCatalogs);

        //	return catalogs;
        //}

        private static void EnsurePluginsFolder()
		{
			Directory.CreateDirectory(PluginsFolder);
		}
	}
}