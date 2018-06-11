using AvalonStudio.Extensibility.Plugin;
using System.Collections.Generic;

namespace AvalonStudio
{
    internal static class ListExtensions
    {
        public static void ConsumeExtension<T>(this List<T> destination, IActivatableExtension extension) where T : class, IActivatableExtension
        {
            if (extension is T)
            {
                destination.Add(extension as T);
            }
        }
    }
}