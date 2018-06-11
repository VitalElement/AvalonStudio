using Splat;
using System;
using System.Collections.Generic;
using System.Composition.Hosting;

namespace AvalonStudio.Extensibility
{    
    public static class IoC
    {
        private static CompositionHost s_compositionHost;

        public static object Get(Type t, string contract = null)
        {
            return s_compositionHost.GetExport(t, contract);
        }

        public static T Get<T>(string contract)
        {
            return s_compositionHost.GetExport<T>(contract);
        }

        public static T Get<T>()
        {
            return s_compositionHost.GetExport<T>();
        }

        public static IEnumerable<T> GetInstances<T>()
        {
            return s_compositionHost.GetExports<T>();
        }

        public static IEnumerable<T> GetInstances<T>(string contract)
        {
            return s_compositionHost.GetExports<T>(contract);
        }

        public static void Initialise (CompositionHost host)
        {
            s_compositionHost = host;
        }
    }
}