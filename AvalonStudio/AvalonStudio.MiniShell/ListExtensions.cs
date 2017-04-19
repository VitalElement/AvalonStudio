namespace AvalonStudio.Shell
{
    using AvalonStudio.Extensibility.Plugin;
    using System.Collections.Generic;
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