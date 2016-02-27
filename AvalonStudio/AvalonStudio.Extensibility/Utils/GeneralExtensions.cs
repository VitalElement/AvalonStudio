namespace AvalonStudio.Utils
{
    using System;
    using System.IO;

    public static class GeneralExtensions
    {
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
