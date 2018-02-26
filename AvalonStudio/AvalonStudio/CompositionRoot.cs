using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;

namespace AvalonStudio
{
    internal static class CompositionRoot
    {
        public static CompositionHost CreateContainer(IEnumerable<IExtensionManifest> extensions)
        {
            var conventions = new ConventionBuilder();
            conventions.ForTypesDerivedFrom<IExtension>().Export<IExtension>();

            // TODO AppDomain here is a custom appdomain from namespace AvalonStudio.Extensibility.Utils. It is able
            // to load any assembly in the bin directory (so not really appdomain) we need to get rid of this
            // once all our default extensions are published with a manifest and copied to extensions dir.
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var extension in extensions)
            {
                foreach (var assembly in extension.GetMefComponents())
                {
                    try
                    {
                        assemblies = assemblies.Append(Assembly.LoadFrom(assembly));
                    }
                    catch (Exception e)
                    {
                        // todo: log exception
                    }
                }
            }

            var configuration = new ContainerConfiguration().WithAssemblies(assemblies, conventions);
            return configuration.CreateContainer();
        }
    }
}