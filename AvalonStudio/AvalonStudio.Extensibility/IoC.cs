using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility
{
    public static class IoC
    {
        public static object Get(Type t, string contract = null)
        {
            return Locator.CurrentMutable.GetService(t, contract);
        }

        public static T Get<T>(string contract = null)
        {
            return (T)Get(typeof(T), contract);
        }

        public static void RegisterConstant<T> (T instance, Type type)
        {
            Locator.CurrentMutable.RegisterConstant(instance, type);
        }
    }
}
