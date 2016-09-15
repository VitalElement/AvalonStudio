using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AvalonStudio.Extensibility
{
    public class ChangingOutput : IDisposable
    {
        private static readonly int ResultLen = 10;

        private static readonly string ClearString = "\r" + new string(' ', Console.WindowWidth - 1) + "\r";
        private static readonly int MaxDescLength = Console.WindowWidth - ResultLen - 4; // [1 - 1 - 2], or bufferExtent - space - brackets
        private readonly string _desc;

        public ChangingOutput(string format, params object[] args)
            : this(string.Format(format, args))
        {

        }

        public ChangingOutput(string test)
        {
            _desc = test.Truncate(MaxDescLength).PadRight(MaxDescLength);
            Print();
        }

        public void Clear()
        {
            Console.Write(ClearString);
        }

        public void Print()
        {
            Clear();
            Console.Write(_desc);
        }

        public void PrintResult(bool passed)
        {
            Print();
            Console.Write(" [");

            var c = Console.ForegroundColor;
            Console.ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write((passed ? "Okay" : "FAILED").Center(ResultLen));
            Console.ForegroundColor = c;

            Console.Write("]");
        }

        public void PrintProgress(double progress)
        {
            var pAmount = (int)(progress * 100) / ResultLen;

            var str = "";
            if (pAmount > 0)
            {
                str = (pAmount == 1 ? "" : new string('=', pAmount - 1)) + ">";
            }

            Print();
            Console.Write(" [{0}]", str.PadRight(ResultLen));
        }

        public void PrintNumber(int num)
        {
            Print();
            Console.Write(" [{0}]", num.ToString(CultureInfo.InvariantCulture).Truncate(ResultLen).Center(ResultLen));
        }

        public void FinishLine()
        {
            Console.Write("\r");
            Console.WriteLine();
        }

        public void Dispose()
        {
            FinishLine();
        }
    }

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

			var sLen = str.Length;
			var cLen = compare.Length;
			var result = new int[sLen + 1, cLen + 1];

			if (sLen == 0)
				return cLen;

			if (cLen == 0)
				return sLen;

			for (int i = 0; i <= sLen; result[i, 0] = i++) ;
			for (int i = 0; i <= cLen; result[0, i] = i++) ;

			for (int i = 1; i <= sLen; i++)
			{
				for (int j = 1; j <= cLen; j++)
				{
					var cost = (compare[j - 1] == str[i - 1]) ? 0 : 1;
					result[i, j] = Math.Min(Math.Min(result[i - 1, j] + 1, result[i, j - 1] + 1), result[i - 1, j - 1] + cost);
				}
			}

			return result[sLen, cLen];
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
				}

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
