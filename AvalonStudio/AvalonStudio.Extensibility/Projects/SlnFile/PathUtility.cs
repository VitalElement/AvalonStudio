using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AvalonStudio.Extensibility.Projects.SlnFile
{
    class PathUtility
    {
        public static string RemoveExtraPathSeparators(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var components = path.Split(Path.DirectorySeparatorChar);
            var result = string.Empty;

            foreach (var component in components)
            {
                if (!string.IsNullOrEmpty(component))
                {
                    result = Path.Combine(result, component);
                }
            }

            if (path[path.Length - 1] == Path.DirectorySeparatorChar)
            {
                result += Path.DirectorySeparatorChar;
            }

            return result;
        }

        public static string GetPathWithForwardSlashes(string path)
        {
            return path.Replace('\\', '/');
        }

        public static string GetPathWithBackSlashes(string path)
        {
            return path.Replace('/', '\\');
        }

        public static string GetPathWithDirectorySeparator(string path)
        {
            if (Path.DirectorySeparatorChar == '/')
            {
                return GetPathWithForwardSlashes(path);
            }
            else
            {
                return GetPathWithBackSlashes(path);
            }
        }
    }
}
