using System;
using System.IO;

namespace AvalonStudio.TextEditor.Utils
{
	/// <summary>
	///     Poor man's template specialization: extension methods for Rope&lt;char&gt;.
	/// </summary>
	public static class CharRope
	{
		/// <summary>
		///     Creates a new rope from the specified text.
		/// </summary>
		public static Rope<char> Create(string text)
		{
			if (text == null)
				throw new ArgumentNullException("text");
			return new Rope<char>(InitFromString(text));
		}

		/// <summary>
		///     Retrieves the text for a portion of the rope.
		///     Runs in O(lg N + M), where M=<paramref name="length" />.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">offset or length is outside the valid range.</exception>
		/// <remarks>
		///     This method counts as a read access and may be called concurrently to other read accesses.
		/// </remarks>
		public static string ToString(this Rope<char> rope, int startIndex, int length)
		{
			if (rope == null)
				throw new ArgumentNullException("rope");
#if DEBUG
			if (length < 0)
				throw new ArgumentOutOfRangeException("length", length, "Value must be >= 0");
#endif
			if (length == 0)
				return string.Empty;
			var buffer = new char[length];
			rope.CopyTo(startIndex, buffer, 0, length);
			return new string(buffer);
		}

		/// <summary>
		///     Retrieves the text for a portion of the rope and writes it to the specified text writer.
		///     Runs in O(lg N + M), where M=<paramref name="length" />.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">offset or length is outside the valid range.</exception>
		/// <remarks>
		///     This method counts as a read access and may be called concurrently to other read accesses.
		/// </remarks>
		public static void WriteTo(this Rope<char> rope, TextWriter output, int startIndex, int length)
		{
			if (rope == null)
				throw new ArgumentNullException("rope");
			if (output == null)
				throw new ArgumentNullException("output");
			rope.VerifyRange(startIndex, length);
			rope.root.WriteTo(startIndex, output, length);
		}

		/// <summary>
		///     Appends text to this rope.
		///     Runs in O(lg N + M).
		/// </summary>
		/// <exception cref="ArgumentNullException">newElements is null.</exception>
		public static void AddText(this Rope<char> rope, string text)
		{
			InsertText(rope, rope.Length, text);
		}

		/// <summary>
		///     Inserts text into this rope.
		///     Runs in O(lg N + M).
		/// </summary>
		/// <exception cref="ArgumentNullException">newElements is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">index or length is outside the valid range.</exception>
		public static void InsertText(this Rope<char> rope, int index, string text)
		{
			if (rope == null)
				throw new ArgumentNullException("rope");
			rope.InsertRange(index, text.ToCharArray(), 0, text.Length);
			/*if (index < 0 || index > rope.Length) {
				throw new ArgumentOutOfRangeException("index", index, "0 <= index <= " + rope.Length.ToString(CultureInfo.InvariantCulture));
			}
			if (text == null)
				throw new ArgumentNullException("text");
			if (text.Length == 0)
				return;
			rope.root = rope.root.Insert(index, text);
			rope.OnChanged();*/
		}

		internal static RopeNode<char> InitFromString(string text)
		{
			if (text.Length == 0)
			{
				return RopeNode<char>.emptyRopeNode;
			}
			var node = RopeNode<char>.CreateNodes(text.Length);
			FillNode(node, text, 0);
			return node;
		}

		private static void FillNode(RopeNode<char> node, string text, int start)
		{
			if (node.contents != null)
			{
				text.CopyTo(start, node.contents, 0, node.length);
			}
			else
			{
				FillNode(node.left, text, start);
				FillNode(node.right, text, start + node.left.length);
			}
		}

		internal static void WriteTo(this RopeNode<char> node, int index, TextWriter output, int count)
		{
			if (node.height == 0)
			{
				if (node.contents == null)
				{
					// function node
					node.GetContentNode().WriteTo(index, output, count);
				}
				else
				{
					// leaf node: append data
					output.Write(node.contents, index, count);
				}
			}
			else
			{
				// concat node: do recursive calls
				if (index + count <= node.left.length)
				{
					node.left.WriteTo(index, output, count);
				}
				else if (index >= node.left.length)
				{
					node.right.WriteTo(index - node.left.length, output, count);
				}
				else
				{
					var amountInLeft = node.left.length - index;
					node.left.WriteTo(index, output, amountInLeft);
					node.right.WriteTo(0, output, count - amountInLeft);
				}
			}
		}

		/// <summary>
		///     Gets the index of the first occurrence of any element in the specified array.
		/// </summary>
		/// <param name="rope">The target rope.</param>
		/// <param name="anyOf">Array of characters being searched.</param>
		/// <param name="startIndex">Start index of the search.</param>
		/// <param name="length">Length of the area to search.</param>
		/// <returns>The first index where any character was found; or -1 if no occurrence was found.</returns>
		public static int IndexOfAny(this Rope<char> rope, char[] anyOf, int startIndex, int length)
		{
			if (rope == null)
				throw new ArgumentNullException("rope");
			if (anyOf == null)
				throw new ArgumentNullException("anyOf");
			rope.VerifyRange(startIndex, length);

			while (length > 0)
			{
				var entry = rope.FindNodeUsingCache(startIndex).PeekOrDefault();
				var contents = entry.node.contents;
				var startWithinNode = startIndex - entry.nodeStartIndex;
				var nodeLength = Math.Min(entry.node.length, startWithinNode + length);
				for (var i = startIndex - entry.nodeStartIndex; i < nodeLength; i++)
				{
					var element = contents[i];
					foreach (var needle in anyOf)
					{
						if (element == needle)
							return entry.nodeStartIndex + i;
					}
				}
				length -= nodeLength - startWithinNode;
				startIndex = entry.nodeStartIndex + nodeLength;
			}
			return -1;
		}

		/// <summary>
		///     Gets the index of the first occurrence of the search text.
		/// </summary>
		public static int IndexOf(this Rope<char> rope, string searchText, int startIndex, int length,
			StringComparison comparisonType)
		{
			if (rope == null)
				throw new ArgumentNullException("rope");
			if (searchText == null)
				throw new ArgumentNullException("searchText");
			rope.VerifyRange(startIndex, length);
			var pos = rope.ToString(startIndex, length).IndexOf(searchText, comparisonType);
			if (pos < 0)
				return -1;
			return pos + startIndex;
		}

		/// <summary>
		///     Gets the index of the last occurrence of the search text.
		/// </summary>
		public static int LastIndexOf(this Rope<char> rope, string searchText, int startIndex, int length,
			StringComparison comparisonType)
		{
			if (rope == null)
				throw new ArgumentNullException("rope");
			if (searchText == null)
				throw new ArgumentNullException("searchText");
			rope.VerifyRange(startIndex, length);
			var pos = rope.ToString(startIndex, length).LastIndexOf(searchText, comparisonType);
			if (pos < 0)
				return -1;
			return pos + startIndex;
		}
	}
}