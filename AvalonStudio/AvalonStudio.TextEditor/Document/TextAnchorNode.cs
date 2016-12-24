using System;

namespace AvalonStudio.TextEditor.Document
{
	/// <summary>
	///     A TextAnchorNode is placed in the TextAnchorTree.
	///     It describes a section of text with a text anchor at the end of the section.
	///     A weak reference is used to refer to the TextAnchor. (to save memory, we derive from WeakReference instead of
	///     referencing it)
	/// </summary>
	internal sealed class TextAnchorNode : WeakReference
	{
		internal bool color;
		internal TextAnchorNode left, right, parent;
		internal int length;
		internal int totalLength; // totalLength = length + left.totalLength + right.totalLength

		public TextAnchorNode(TextAnchor anchor) : base(anchor)
		{
		}

		internal TextAnchorNode LeftMost
		{
			get
			{
				var node = this;
				while (node.left != null)
					node = node.left;
				return node;
			}
		}

		internal TextAnchorNode RightMost
		{
			get
			{
				var node = this;
				while (node.right != null)
					node = node.right;
				return node;
			}
		}

		/// <summary>
		///     Gets the inorder successor of the node.
		/// </summary>
		internal TextAnchorNode Successor
		{
			get
			{
				if (right != null)
				{
					return right.LeftMost;
				}
				var node = this;
				TextAnchorNode oldNode;
				do
				{
					oldNode = node;
					node = node.parent;
					// go up until we are coming out of a left subtree
				} while (node != null && node.right == oldNode);
				return node;
			}
		}

		/// <summary>
		///     Gets the inorder predecessor of the node.
		/// </summary>
		internal TextAnchorNode Predecessor
		{
			get
			{
				if (left != null)
				{
					return left.RightMost;
				}
				var node = this;
				TextAnchorNode oldNode;
				do
				{
					oldNode = node;
					node = node.parent;
					// go up until we are coming out of a right subtree
				} while (node != null && node.left == oldNode);
				return node;
			}
		}

		public override string ToString()
		{
			return "[TextAnchorNode Length=" + length + " TotalLength=" + totalLength + " Target=" + Target + "]";
		}
	}
}