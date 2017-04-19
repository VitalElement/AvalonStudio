using Avalonia;
using System;
using System.Collections.Generic;
using System.IO;

namespace AvalonStudio.Utils
{
    public static class GeneralExtensions
    {
        public static bool IsFileLocked(this FileInfo file)
        {
            try
            {
                using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        public static double DistanceTo(this Point p, Point q)
        {
            var a = p.X - q.X;
            var b = p.Y - q.Y;
            var distance = Math.Sqrt((a * a) + (b * b));
            return distance;
        }

        public static T BinarySearch<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey key)
            where TKey : IComparable<TKey>
        {
            var min = 0;
            var max = list.Count;
            while (min < max)
            {
                var mid = min + ((max - min) / 2);
                var midItem = list[mid];
                var midKey = keySelector(midItem);
                var comp = midKey.CompareTo(key);
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
                keySelector(list[min]).CompareTo(key) == 0)
            {
                return list[min];
            }

            return default(T);
        }

        public static T BinarySearch<T, TKey>(this IList<T> list, TKey key)
            where T : IComparable<TKey>
        {
            var min = 0;
            var max = list.Count;
            while (min < max)
            {
                var mid = min + (max - min) / 2;
                var midItem = list[mid];

                var comp = midItem.CompareTo(key);
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
                list[min].CompareTo(key) == 0)
            {
                return list[min];
            }

            return default(T);
        }

        public static int BinarySearchIndexOf<T, TKey>(this IList<T> list, TKey value) where T : IComparable<TKey>
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            int lower = 0;
            int upper = list.Count - 1;

            while (lower <= upper)
            {
                int middle = lower + ((upper - lower) / 2);
                int comparisonResult = list[middle].CompareTo(value);

                if (comparisonResult < 0)
                {
                    lower = middle + 1;
                }
                else if (comparisonResult > 0)
                {
                    upper = middle - 1;
                }
                else
                {
                    return middle;
                }
            }

            return ~lower;
        }

        /// <summary>
        ///     Inserts an element into the collection, keeping it sorted. The collection must be sorted
        ///     already, i.e. populated only with this method. The template type for the collection must
        ///     implement IComparable.
        /// </summary>
        /// <typeparam name="T">is the type of items in the collection.</typeparam>
        /// <param name="myself">is "this" reference.</param>
        /// <param name="item">is the item to insert.</param>
        public static void InsertSorted<T>(this IList<T> myself, T item, bool exclusive = true) where T : IComparable<T>
        {
            var index = myself.BinarySearchIndexOf(item);

            if (index < 0)
            {
                myself.Insert(~index, item);
            }
            else if (!exclusive)
            {
                myself.Insert(index, item);
            }
        }

        /// <summary>
        ///     Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static string MakeRelativePath(this string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            var fromUri = new Uri(fromPath);
            var toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme)
            {
                return toPath;
            } // path can't be made relative.

            var relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.ToUpperInvariant() == "FILE")
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        public static bool IsPunctuationChar(this char c)
        {
            var result = false;

            switch (c)
            {
                case '"':
                case '\'':
                case '.':
                case '/':
                    result = true;
                    break;
            }

            return result;
        }

        public static bool IsOpenBracketChar(this char c)
        {
            var result = false;

            switch (c)
            {
                //case '<':
                case '(':
                case '{':
                case '[':
                case '"':
                case '\'':
                    result = true;
                    break;
            }

            return result;
        }

        public static char GetOpenBracketChar(this char c)
        {
            if (!c.IsCloseBracketChar())
            {
                throw new Exception("Character is not supported as bracket.");
            }

            var result = '(';

            switch (c)
            {
                case ')':
                    result = '(';
                    break;

                case '>':
                    result = '<';
                    break;

                case ']':
                    result = '[';
                    break;

                case '}':
                    result = '{';
                    break;

                case '\'':
                    result = '\'';
                    break;

                case '"':
                    result = '"';
                    break;
            }

            return result;
        }

        public static bool IsSymbolChar(this char letter)
        {
            bool result = false;

            if (char.IsLetter(letter) || letter == '_')
            {
                result = true;
            }

            return result;
        }

        public static char GetCloseBracketChar(this char c)
        {
            if (!c.IsOpenBracketChar())
            {
                throw new Exception("Character is not supported as bracket.");
            }

            var result = ')';

            switch (c)
            {
                case '(':
                    result = ')';
                    break;

                case '<':
                    result = '>';
                    break;

                case '[':
                    result = ']';
                    break;

                case '{':
                    result = '}';
                    break;

                case '\'':
                    result = '\'';
                    break;

                case '"':
                    result = '"';
                    break;
            }

            return result;
        }

        public static bool IsWhiteSpace(this char c)
        {
            return char.IsWhiteSpace(c);
        }

        public static bool IsCloseBracketChar(this char c)
        {
            var result = false;

            switch (c)
            {
                //case '>':
                case ')':
                case '}':
                case ']':
                case '"':
                case '\'':
                    result = true;
                    break;
            }

            return result;
        }
    }
}