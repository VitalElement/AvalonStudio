namespace AvalonStudio.Utils
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;

    public static class Extensions
    {
        public static string GetDescription<T> (this T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType ();
            if (!type.IsEnum)
            {
                throw new ArgumentException ("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo [] memberInfo = type.GetMember (enumerationValue.ToString ());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object [] attrs = memberInfo [0].GetCustomAttributes (typeof (DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs [0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString ();

        }

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static String MakeRelativePath (this String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty (fromPath)) throw new ArgumentNullException ("fromPath");
            if (String.IsNullOrEmpty (toPath)) throw new ArgumentNullException ("toPath");

            Uri fromUri = new Uri (fromPath);
            Uri toUri = new Uri (toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri (toUri);
            String relativePath = Uri.UnescapeDataString (relativeUri.ToString ());

            if (toUri.Scheme.ToUpperInvariant () == "FILE")
            {
                relativePath = relativePath.Replace (Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        private static bool osDetected = false;
        private static bool isUnix = false;
        public static bool IsUnix
        {
            get
            {
                if (!osDetected)
                {
                    osDetected = true;

                    OperatingSystem os = Environment.OSVersion;
                    PlatformID pid = os.Platform;
                    switch (pid)
                    {
                        case PlatformID.Win32NT:
                        case PlatformID.Win32S:
                        case PlatformID.Win32Windows:
                        case PlatformID.WinCE:
                            break;

                        default:
                            isUnix = true;
                            break;
                    }
                }
                return isUnix;
            }
        }

        public static string ConvertPathForOS (this string path)
        {
            if (IsUnix)
            {
                path = path.Replace("..\\", "../").Replace("\\", "/");
            }

            return path;
        }

        public static T BinarySearch<T, TKey> (this IList<T> list, Func<T, TKey> keySelector, TKey key)
        where TKey : IComparable<TKey>
        {
            int min = 0;
            int max = list.Count;
            while (min < max)
            {
                int mid = min + ((max - min) / 2);
                T midItem = list [mid];
                TKey midKey = keySelector (midItem);
                int comp = midKey.CompareTo (key);
                if (comp < 0)
                {
                    min = mid + 1;
                }
                else if (comp > 0)
                {
                    max = mid - 1;
                }
                else
                {
                    return midItem;
                }
            }

            if (min == max && min < list.Count &&
                keySelector (list [min]).CompareTo (key) == 0)
            {
                return list [min];
            }

            return default (T);
        }
    }
}
