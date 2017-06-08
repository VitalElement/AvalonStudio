using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AvalonStudio.Extensibility.Utils
{
    public class AppDomain
    {
        public static AppDomain CurrentDomain { get; private set; }
        private static Assembly[] s_assemblies;

        static AppDomain()
        {
            CurrentDomain = new AppDomain();
        }

        public Assembly[] GetAssemblies()
        {
            if (s_assemblies == null)
            {
                var assemblies = new List<Assembly>();

                var compileDependencies = DependencyContext.Default.CompileLibraries;

                foreach (var library in compileDependencies)
                {
                    if (IsCandidateCompilationLibrary(library))
                    {
                        try
                        {
                            var assembly = Assembly.Load(new AssemblyName(library.Name));
                            assemblies.Add(assembly);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                s_assemblies = assemblies.ToArray();
            }

            return s_assemblies;
        }

        private static bool IsCandidateCompilationLibrary(Library compilationLibrary)
        {
            return compilationLibrary.Name.ToLower() == "avalonStudio"
                || compilationLibrary.Name.ToLower().StartsWith("avalonstudio")
                || compilationLibrary.Dependencies.Any(d => d.Name.ToLower().StartsWith("avalonstudio"));
        }
    }
}