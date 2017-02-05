using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AvalonStudio.Extensibility.Utils
{
    public class AppDomain
    {
        public static AppDomain CurrentDomain { get; private set; }

        static AppDomain()
        {
            CurrentDomain = new AppDomain();
        }

        public Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();

            var compileDependencies = DependencyContext.Default.CompileLibraries;

            foreach (var library in compileDependencies)
            {
                if (IsCandidateCompilationLibrary(library))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }

            return assemblies.ToArray();
        }

        private static bool IsCandidateCompilationLibrary(Library compilationLibrary)
        {
            return compilationLibrary.Name.ToLower() == ("avalonStudio")
                || compilationLibrary.Dependencies.Any(d => d.Name.ToLower().StartsWith("avalonstudio"));
        }
    }
}
