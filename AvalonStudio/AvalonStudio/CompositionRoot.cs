﻿namespace AvalonStudio
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Languages;
    using Projects;
    internal static class CompositionRoot
    {
        private static readonly string PluginsFolder = "Plugins";

        public static CompositionContainer CreateContainer()
        {
            EnsurePluginsFolder();
            
            var catalogs = GetCatalogsToImport();

            return new CompositionContainer(new AggregateCatalog(catalogs));
        }

        private static List<ComposablePartCatalog> GetCatalogsToImport()
        {
            var pluginsCatalog = new DirectoryCatalog(PluginsFolder);
            var assemblyCatalogs = ScannedAssemblies.Select(assembly => new AssemblyCatalog(assembly));

            var catalogs = new List<ComposablePartCatalog>();
            catalogs.Add(pluginsCatalog);
            catalogs.AddRange(assemblyCatalogs);

            return catalogs;
        }

        private static IEnumerable<Assembly> ScannedAssemblies
        {
            get
            {
                return new[]
                {
                    typeof (App).GetTypeInfo().Assembly,
                    typeof (ILanguageService).GetTypeInfo().Assembly,                    
                };
            }
        }

        private static void EnsurePluginsFolder()
        {
            Directory.CreateDirectory(PluginsFolder);
        }
    }
}