namespace AvalonStudio.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Extensions
    {
        public static string NormalizePath(this string path)
        {
            if (path != null)
            {
                return Path.GetFullPath(new Uri(path).LocalPath)
                           .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                           .ToUpperInvariant();
            }
            else
            {
                return null;
            }
        }
    }
}
