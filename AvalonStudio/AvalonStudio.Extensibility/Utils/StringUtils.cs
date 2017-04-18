using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AvalonStudio.Extensibility
{
    public static class Ext
    {
        public static string StripHTML(this string HTMLText)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var stripped = reg.Replace(HTMLText, "");
            return stripped;
        }

        public static string Center(this string stringToCenter, int totalLength)
        {
            return stringToCenter
                .PadLeft(((totalLength - stringToCenter.Length) / 2) + stringToCenter.Length)
                .PadRight(totalLength);
        }

        public static string Truncate(this string value, int maxLength, string endString = "")
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return $"{(value.Length <= maxLength ? value : value.Substring(0, maxLength))}{endString}";
        }

        public static int LevenshteinDistance(this string str, string compare, bool caseSensitive = true)
        {
            if (!caseSensitive)
            {
                str = str.ToLower();
                compare = compare.ToLower();
            }

            var length = str.Length;
            var compareLength = compare.Length;
            var result = new int[length + 1, compareLength + 1];

            if (length == 0)
                return compareLength;

            if (compareLength == 0)
                return length;

            for (int i = 0; i <= length; result[i, 0] = i++)
            {
            }

            for (int i = 0; i <= compareLength; result[0, i] = i++)
            {
            }

            for (int i = 1; i <= length; i++)
            {
                for (int j = 1; j <= compareLength; j++)
                {
                    var cost = (compare[j - 1] == str[i - 1]) ? 0 : 1;
                    result[i, j] = Math.Min(Math.Min(result[i - 1, j] + 1, result[i, j - 1] + 1), result[i - 1, j - 1] + cost);
                }
            }

            return result[length, compareLength];
        }

        public static IEnumerable<string> Chunkify(this string str, int maxChunkLength)
        {
            return str.Chunkify(maxChunkLength, " ".ToCharArray(), "-".ToCharArray());
        }

        public static IEnumerable<string> Chunkify(this string str, int maxChunkLength, char[] removedSplitters, char[] keptSplitters)
        {
            var splitters = removedSplitters.Concat(keptSplitters).ToArray();

            var startIndex = 0;

            while (startIndex < str.Length)
            {
                // Calculate the maximum length of this chunk.
                var maxIndex = Math.Min(startIndex + maxChunkLength, str.Length) - 1;

                // Try to make a chunk this big.
                var endIndex = maxIndex;

                if (!splitters.Contains(str[endIndex]) && (endIndex != str.Length - 1 && !splitters.Contains(str[endIndex + 1])))
                {
                    // If the last char in our chunk is part of a word,
                    // Try to find the start of the word
                    endIndex = str.LastIndexOfAny(splitters, maxIndex);

                    if (endIndex < startIndex) // We didn't find one in bounds
                        endIndex = maxIndex; // So we have to return to splitting the word               

                    // Make our chunk. We'll leave splitters at the start, if they exist.
                    var chunk = str.Substring(startIndex, endIndex - startIndex + 1).TrimEnd(removedSplitters);

                    // If we get a chunk that's all removed splitters, don't output it
                    if (chunk.Length != 0)
                        yield return chunk;

                    // Start on the next chunk
                    startIndex = endIndex + 1;
                }
            }
        }
    }
