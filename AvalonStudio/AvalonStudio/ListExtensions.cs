using AvalonStudio.Extensibility.Plugin;
using System.Collections.Generic;

namespace AvalonStudio
{
    internal static class ListExtensions
    {
        public static void ConsumeExtension<T>(this List<T> destination, IExtension extension) where T : class, IExtension
        {
            if (extension is T)
            {
                destination.Add(extension as T);
            }
        }
    }
}