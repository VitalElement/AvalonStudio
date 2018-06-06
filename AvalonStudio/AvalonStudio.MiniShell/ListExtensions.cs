namespace AvalonStudio.Shell
{
    using AvalonStudio.Extensibility.Plugin;
    using System.Collections.Generic;
    internal static class ListExtensions
    {
        public static void ConsumeExtension<T>(this List<T> destination, IActivatable extension) where T : class, IActivatable
        {
            if (extension is T)
            {
                destination.Add(extension as T);
            }
        }
    }
}