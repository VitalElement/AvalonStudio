namespace AvalonStudio.Utils
{
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class GeneralExtensions
    {
        public static double DistanceTo(this Point p, Point q)
        {
            double a = p.X - q.X;
            double b = p.Y - q.Y;
            double distance = Math.Sqrt(a * a + b * b);
            return distance;
        }

        public static T BinarySearch<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey key)
            where TKey : IComparable<TKey>
        {
            int min = 0;
            int max = list.Count;
            while (min < max)
            {
                int mid = min + ((max - min) / 2);
                T midItem = list[mid];
                TKey midKey = keySelector(midItem);
                int comp = midKey.CompareTo(key);
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

        /// <summary>
        /// Inserts an element into the collection, keeping it sorted. The collection must be sorted
        /// already, i.e. populated only with this method. The template type for the collection must
        /// implement IComparable.
        /// </summary>
        /// <typeparam name="T">is the type of items in the collection.</typeparam>
        /// <param name="myself">is "this" reference.</param>
        /// <param name="item">is the item to insert.</param>
        public static void InsertSorted<T>(this IList<T> myself, T item) where T : IComparable<T>
        {
            if (myself.Count == 0)
            {
                myself.Add(item);
            }
            else
            {
                bool last = true;

                for (int i = 0; i < myself.Count; i++)
                {
                    int result = myself[i].CompareTo(item);

                    if (result >= 1)
                    {
                        myself.Insert(i, item);

                        last = false;

                        break;
                    }
                }

                if (last)
                {
                    myself.Add(item);
                }
            }
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
        public static String MakeRelativePath(this String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.ToUpperInvariant() == "FILE")
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        public static bool IsPunctuationChar(this char c)
        {
            bool result = false;

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
            bool result = false;

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

            char result = '(';

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

        public static char GetCloseBracketChar(this char c)
        {
            if (!c.IsOpenBracketChar())
            {
                throw new Exception("Character is not supported as bracket.");
            }

            char result = ')';

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
            bool result = false;

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
